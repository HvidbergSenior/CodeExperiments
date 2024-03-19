using Insight.BuildingBlocks.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Customers.Application.GetPossibleCustomerPermissions;

public static class GetPossibleCustomerPermissionsEndpoint
{
    public static void MapGetPossibleCustomerPermissions(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    CustomerEndpointUrls.GET_POSSIBLE_CUSTOMER_PERMISSIONS,
                    async (IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetPossibleCustomerPermissionsQuery.Create();
                        var results = await queryBus
                            .Send<GetPossibleCustomerPermissionsQuery,
                                GetPossibleCustomerPermissionsResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .Produces<GetPossibleCustomerPermissionsResponse>()
                .RequireAuthorization()
                .WithName("GetPossibleCustomerPermissions")
                .WithTags("Customers");
        }
}