using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.GetAllUsers
{
    public static class GetAllUsersEndpoint
    {
        public static void MapGetAllUsersEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(UserEndpointUrls.GET_ALL_USERS_ENDPOINT, async (int page,
                        int pageSize,
                        bool isOrderDescending,
                        string orderByProperty,
                        UserStatus status,
                        string? customerName,
                        string? customerNumber,
                        string? email,
                        IQueryBus queryBus,
                    CancellationToken cancellationToken) =>
                {
                    var paginationParameters = BuildingBlocks.Domain.PaginationParameters.Create(page, pageSize);
                    var sortingParameters = BuildingBlocks.Domain.SortingParameters.Create(isOrderDescending: isOrderDescending, orderByProperty: orderByProperty);

                    var emailFilter = !string.IsNullOrWhiteSpace(email) ? EmailFiltering.Create(email) : EmailFiltering.Empty();
                    var customerNameFilter = !string.IsNullOrWhiteSpace(customerName) ? CustomerName.Create(customerName) : CustomerName.Empty();
                    var customerNumberFilter = !string.IsNullOrWhiteSpace(customerNumber) ? CustomerNumber.Create(customerNumber) : CustomerNumber.Empty();

                    var filteringParameters = FilteringParameters.Create(emailFilter, status, customerNameFilter, customerNumberFilter);
                    
                    var query = GetAllUsersQuery.Create(paginationParameters, sortingParameters, filteringParameters);
                    var result = await queryBus.Send<GetAllUsersQuery, GetAllUsersResponse>(query, cancellationToken);

                    return Results.Ok(result);
                })
                .RequireAuthorization()
                .Produces<GetAllUsersResponse>()
                .WithName("GetAllUsers")
                .WithTags("Users");
        }
    }
}
