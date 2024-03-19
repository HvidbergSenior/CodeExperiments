using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.UserAccess.Application.GetAllUsersForAdmin
{
    public static class GetAllUsersAdminEndpoint
    {
        public static void MapGetAllUsersAdminEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(UserAdministrationEndpointUrls.GET_ALL_USERS_ADMIN_ENDPOINT, async (int page,
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
                    var sortingParameters =
                        BuildingBlocks.Domain.SortingParameters.Create(isOrderDescending: isOrderDescending,
                            orderByProperty: orderByProperty);

                    var emailFilter = !string.IsNullOrWhiteSpace(email) ? EmailFiltering.Create(email) : EmailFiltering.Empty();
                    var customerNameFilter = !string.IsNullOrWhiteSpace(customerName) ? CustomerName.Create(customerName) : CustomerName.Empty();
                    var customerNumberFilter = !string.IsNullOrWhiteSpace(customerNumber) ? CustomerNumber.Create(customerNumber) : CustomerNumber.Empty();
                    
                    var filteringParameters = FilteringParameters.Create(emailFilter, status, customerNameFilter, customerNumberFilter);
                    
                    var query = GetAllUsersAdminQuery.Create(paginationParameters, sortingParameters, filteringParameters);
                    var result = await queryBus.Send<GetAllUsersAdminQuery, GetAllUsersAdminResponse>(query, cancellationToken);

                    return Results.Ok(result);
                })
                .RequireAuthorization()
                .Produces<GetAllUsersAdminResponse>()
                .WithName("GetAllUsersAdmin")
                .WithTags("Administrationusers");
        }
    }
}
