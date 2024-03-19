using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Integration.GetExtendedFlattenedCustomerPermissionsWithChildren;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;


namespace Insight.UserAccess.Application
{
    public sealed class AuthenticatedExecutionContext : IExecutionContext
    {
        private const int CACHE_ENTRY_LIFE_TIME_IN_HOURS = 1;
        private const string MEMORY_CACHE_USER_PREFIX = "UserName_";
        private const string MEMORY_CACHE_EXTENDEDPERMISSIONS_PREFIX = "UserNameExtendedPermissions_";
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;
        private readonly IMemoryCache memoryCache;
        private readonly IQueryBus queryBus;

        public AuthenticatedExecutionContext(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository, IMemoryCache memoryCache, IQueryBus queryBus)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.memoryCache = memoryCache;
            this.queryBus = queryBus;
        }

        public Guid UserId => throw new NotImplementedException();

        private string? UserNameFromIdentity => httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public string UserName => UserNameFromIdentity ?? "Unknown";

        // TODO : Should this be cashed on the context, to avoid multiple calls to the userRepository ?
        private async Task<User?> GetUserAsync(CancellationToken cancellationToken = default)
        {
            var userName = UserNameFromIdentity;

            if (userName == null)
            {
                return null;
            }

            return await memoryCache.GetOrCreateAsync(MEMORY_CACHE_USER_PREFIX + userName, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(CACHE_ENTRY_LIFE_TIME_IN_HOURS);
                return await userRepository.FindByUserName(userName,
                                       httpContextAccessor.HttpContext?.RequestAborted ?? cancellationToken);
            });
        }

        public async Task<ReadOnlyCollection<CustomerPermissionGroup>> GetCustomersPermissionsAsync(bool includeChildren = false, CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(CancellationToken.None);
            if (user == null)
            {
                return new List<CustomerPermissionGroup>().AsReadOnly();
            }

            if(!includeChildren)
            {
                return user?.CustomerPermissions?.AsReadOnly() ?? new List<CustomerPermissionGroup>().AsReadOnly();
            }

            var userName = user.UserName.Value;
            var permissionsExtended = await memoryCache.GetOrCreateAsync(MEMORY_CACHE_EXTENDEDPERMISSIONS_PREFIX + userName, async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddHours(CACHE_ENTRY_LIFE_TIME_IN_HOURS);
                var permissionsForUserExtended = await queryBus.Send<GetExtendedFlattenedCustomerPermissionsWithChildrenQuery, GetExtendedFlattenedCustomerPermissionsWithChildrenDto>(GetExtendedFlattenedCustomerPermissionsWithChildrenQuery.Create(user?.CustomerPermissions ?? new List<CustomerPermissionGroup>()), cancellationToken);

                return permissionsForUserExtended?.CustomerPermissionsExtended?.AsReadOnly();
            });

            return permissionsExtended ?? new List<CustomerPermissionGroup>().AsReadOnly();
        }

        public async Task<bool> GetAdminPrivileges(CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(cancellationToken);
            return user != null && user.AdminPrivileges.Value;
        }
    }

}