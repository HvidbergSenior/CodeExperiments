using Insight.BuildingBlocks.Application.Queries;
using Insight.Customers.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Customers.Application.GetPossibleCustomerPermissionsForGivenUser;

public static class GetPossibleCustomerPermissionsForGivenUserEndpoint
{
    public static void MapGetPossibleCustomerPermissionsForGivenUser(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    CustomerEndpointUrls.GET_POSSIBLE_CUSTOMER_PERMISSIONS_FOR_GIVEN_USER,
                    async (IQueryBus queryBus,
                        string userName,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetPossibleCustomerPermissionsForGivenUserQuery.Create(UserName.Create(userName));
                        var results = await queryBus
                            .Send<GetPossibleCustomerPermissionsForGivenUserQuery,
                                GetPossibleCustomerPermissionsForGivenUserResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .Produces<GetPossibleCustomerPermissionsForGivenUserResponse>()
                .RequireAuthorization()
                .WithName("GetPossibleCustomerPermissionsForGivenUser")
                .WithTags("Customers");
        }
}