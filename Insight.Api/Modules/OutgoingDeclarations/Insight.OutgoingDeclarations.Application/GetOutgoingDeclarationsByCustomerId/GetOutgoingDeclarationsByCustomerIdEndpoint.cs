using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId
{
    public static class GetOutgoingDeclarationsByCustomerIdEndpoint
    {
        public static void MapGetOutgoingDeclarationsByCustomerIdEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATIONS_BY_CUSTOMER_ID_ENDPOINT,
                    async (
                        string customerId,
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetOutgoingDeclarationsByCustomerIdQuery.Create(CustomerNumber.Create(customerId));

                        var results = await queryBus
                            .Send<GetOutgoingDeclarationsByCustomerIdQuery,
                                GetOutgoingDeclarationsByCustomerIdResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetOutgoingDeclarationsByCustomerIdResponse>()
                .WithName("GetOutgoingDeclarationsByCustomerId")
                .WithTags("OutgoingDeclarations");
        }
    }
}