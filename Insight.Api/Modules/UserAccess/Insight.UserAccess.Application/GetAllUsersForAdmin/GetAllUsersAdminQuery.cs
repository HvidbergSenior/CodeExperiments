using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;
using JasperFx.Core;

namespace Insight.UserAccess.Application.GetAllUsersForAdmin
{
    public sealed class GetAllUsersAdminQuery : IQuery<GetAllUsersAdminResponse>
    {
        public PaginationParameters PaginationParameters { get; private set; } = PaginationParameters.Empty();
        public SortingParameters SortingParameters { get; private set; } = SortingParameters.Empty();
        public FilteringParameters FilteringParameters { get; private set; } = FilteringParameters.Empty();

        private GetAllUsersAdminQuery(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            FilteringParameters = filteringParameters;
        }

        public static GetAllUsersAdminQuery Create(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            return new GetAllUsersAdminQuery(paginationParameters, sortingParameters, filteringParameters);
        }
    }

    internal class GetAllUsersAdminQueryHandler : IQueryHandler<GetAllUsersAdminQuery, GetAllUsersAdminResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IExecutionContext executionContext;

        public GetAllUsersAdminQueryHandler(IUserRepository userRepository, IExecutionContext executionContext)
        {
            this.userRepository = userRepository;
            this.executionContext = executionContext;
        }

        public async Task<GetAllUsersAdminResponse> Handle(GetAllUsersAdminQuery query, CancellationToken cancellationToken)
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

            return new GetAllUsersAdminResponse(mappedUsers, hasMore, totalUserCount);
        }

        private IReadOnlyList<GetAllUsersAdminResponse.AllUserAdminResponse> MapToUserResponse(GetAllUsersAdminQuery query, IEnumerable<User> users)
        {
            var usersDict = new Dictionary<string, GetAllUsersAdminResponse.AllUserAdminResponse>();

            var userResponses = new List<GetAllUsersAdminResponse.AllUserAdminResponse>();

            foreach (var user in users.Where(c=> query.FilteringParameters.CustomerName.Value.IsEmpty() || c.CustomerPermissions.Any(o=> o.CustomerName.Value.ToUpperInvariant().Contains(query.FilteringParameters.CustomerName.Value.ToUpperInvariant()))))
            {
                var userRole = user.AdminPrivileges.Value ? UserRole.Admin : UserRole.User;

                if (user.CustomerPermissions.Count == 0
                    && query.FilteringParameters.CustomerName.Value.IsEmpty() && query.FilteringParameters.CustomerNumber.Value.IsEmpty())
                {
                    //If no permissions and no filter on customers still return
                    userResponses.Add(new GetAllUsersAdminResponse.AllUserAdminResponse(
                        user.UserId.Value.ToString(),
                        user.UserName.Value,
                        user.Email.Value,
                        userRole == UserRole.Admin,
                        userRole == UserRole.Admin,
                        userRole == UserRole.Admin,
                        userRole,
                        user.Blocked.Value,
                        user.FirstName.Value,
                        user.LastName.Value
                    ));
                }
                else
                {

                    if (usersDict.TryAdd(user.UserId.Value.ToString(), new GetAllUsersAdminResponse.AllUserAdminResponse(user.UserId.Value.ToString(),
                                                                                                                            user.UserName.Value,
                                                                                                                            user.Email.Value,
                                                                                                                            userRole == UserRole.Admin || user.CustomerPermissions.Any(o=> o.Permissions.Any(p => p == CustomerPermission.FuelConsumption)),
                                                                                                                            userRole == UserRole.Admin || user.CustomerPermissions.Any(o => o.Permissions.Any(p => p == CustomerPermission.SustainabilityReport)),
                                                                                                                            userRole == UserRole.Admin || user.CustomerPermissions.Any(o => o.Permissions.Any(p => p == CustomerPermission.FleetManagement)),
                                                                                                                            userRole,
                                                                                                                            user.Blocked.Value,
                                                                                                                            user.FirstName.Value,
                                                                                                                            user.LastName.Value)))
                    {
                        userResponses.Add(usersDict[user.UserId.Value.ToString()]);
                    }                    
                }
            }

            return userResponses;

        }
    }
    internal class GetAllUsersAdminQueryAuthorizer : IAuthorizer<GetAllUsersAdminQuery>
    {
        public Task<AuthorizationResult> Authorize(GetAllUsersAdminQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}