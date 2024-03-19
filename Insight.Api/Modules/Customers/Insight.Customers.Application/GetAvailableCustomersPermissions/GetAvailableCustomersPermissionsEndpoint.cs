using Insight.BuildingBlocks.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Customers.Application.GetAvailableCustomersPermissions;

public static class GetAvailableCustomersPermissionsEndpoint
{
    public static void MapGetAvailableCustomersPermissions(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    CustomerEndpointUrls.GET_AVAILABLE_CUSTOMERS_PERMISSIONS,
                    async (IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetAvailableCustomersPermissionsQuery.Create();
                        var results = await queryBus
                            .Send<GetAvailableCustomersPermissionsQuery,
                                GetAvailableCustomersPermissionsResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .Produces<GetAvailableCustomersPermissionsResponse>()
                .RequireAuthorization()
                .WithName("GetAvailableCustomersPermissions")
                .WithTags("Customers");
        }
}