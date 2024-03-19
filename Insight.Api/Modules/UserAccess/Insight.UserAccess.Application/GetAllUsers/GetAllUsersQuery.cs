using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Application.GetAllUsersForAdmin;
using Insight.UserAccess.Domain.User;
using JasperFx.Core;

namespace Insight.UserAccess.Application.GetAllUsers
{
    public sealed class GetAllUsersQuery : IQuery<GetAllUsersResponse>
    {
        public PaginationParameters PaginationParameters { get; private set; } = PaginationParameters.Empty();
        public SortingParameters SortingParameters { get; private set; } = SortingParameters.Empty();
        public FilteringParameters FilteringParameters { get; private set; } = FilteringParameters.Empty();

        private GetAllUsersQuery(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            FilteringParameters = filteringParameters;
        }

        public static GetAllUsersQuery Create(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            return new GetAllUsersQuery(paginationParameters, sortingParameters, filteringParameters);
        }
    }

    internal class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IExecutionContext executionContext;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IExecutionContext executionContext)
        {
            this.userRepository = userRepository;
            this.executionContext = executionContext;
        }

        public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerId>? adminForCustomerIds;
            if (await executionContext.GetAdminPrivileges())
            {
                adminForCustomerIds = null;
            }
            else
            {
                var myPermissions =
                    await executionContext.GetCustomersPermissionsAsync(cancellationToken: cancellationToken);

                adminForCustomerIds = myPermissions.Where(p => p.Permissions.Contains(CustomerPermission.Admin))
                    .Select(p => p.CustomerId).ToList();
            }
            var (users, totalUserCount, hasMore) = await userRepository.GetAllUsersWithPermissionsForCustomersAsync(
                adminForCustomerIds, query.PaginationParameters, query.SortingParameters, query.FilteringParameters,
                cancellationToken);

            var mappedUsers = MapToUserResponse(query, users);

            return new GetAllUsersResponse(mappedUsers, hasMore, totalUserCount);
        }

        private IReadOnlyList<GetAllUsersResponse.AllUserResponse> MapToUserResponse(GetAllUsersQuery query, IEnumerable<User> users)
        {
            var usersDict = new Dictionary<string, GetAllUsersResponse.AllUserResponse>();
            var userResponses = new List<GetAllUsersResponse.AllUserResponse>();
            
            foreach (var user in users.Where(c => (query.FilteringParameters.CustomerName.Value.IsEmpty() && query.FilteringParameters.CustomerNumber.Value.IsEmpty()) || c.CustomerPermissions.Any(o => o.CustomerName.Value.ToUpperInvariant().Contains(query.FilteringParameters.CustomerName.Value.ToUpperInvariant())) || c.CustomerPermissions.Any(o => o.CustomerNumber.Value.ToUpperInvariant().Contains(query.FilteringParameters.CustomerNumber.Value.ToUpperInvariant()))))
            {
                var userRole = user.AdminPrivileges.Value ? UserRole.Admin : UserRole.User;

                if (user.CustomerPermissions.Count == 0
                    && query.FilteringParameters.CustomerName.Value.IsEmpty() && query.FilteringParameters.CustomerNumber.Value.IsEmpty())
                {
                    //If no permissions and no filter on customers still return
                    userResponses.Add(new GetAllUsersResponse.AllUserResponse(user.UserId.Value.ToString(),
                        user.FirstName.Value,
                        user.LastName.Value,
                        user.UserName.Value,
                        user.Email.Value,
                        userRole == UserRole.Admin,
                        userRole == UserRole.Admin,
                        userRole == UserRole.Admin,
                        userRole,
                        user.Blocked.Value));
                }
                else
                {   
                    if (usersDict.TryAdd(user.UserId.Value.ToString(), new GetAllUsersResponse.AllUserResponse(user.UserId.Value.ToString(),
                                user.FirstName.Value,
                                user.LastName.Value,
                                user.UserName.Value,
                                user.Email.Value,
                                userRole == UserRole.Admin || user.CustomerPermissions.Any(o => o.Permissions.Any(p => p == CustomerPermission.FuelConsumption)),
                                userRole == UserRole.Admin || user.CustomerPermissions.Any(o => o.Permissions.Any(p => p == CustomerPermission.SustainabilityReport)),
                                userRole == UserRole.Admin || user.CustomerPermissions.Any(o => o.Permissions.Any(p => p == CustomerPermission.FleetManagement)),
                                userRole,
                                user.Blocked.Value
                            )))
                    {
                        userResponses.Add(usersDict[user.UserId.Value.ToString()]);
                    }
                }
            }

            return userResponses;

        }
    }
    internal class GetAllUsersQueryAuthorizer : IAuthorizer<GetAllUsersQuery>
    {
        public Task<AuthorizationResult> Authorize(GetAllUsersQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}