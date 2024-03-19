using Insight.BuildingBlocks.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations
{
    public static class GetOutgoingDeclarationsEndpoint
    {
        public static void MapGetOutgoingDeclarationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATIONS_ENDPOINT,
                    async (
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetOutgoingDeclarationsQuery.Create();

                        var results = await queryBus
                            .Send<GetOutgoingDeclarationsQuery,
                                GetOutgoingDeclarationsResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetOutgoingDeclarationsResponse>()
                .WithName("GetOutgoingDeclarations")
                .WithTags("OutgoingDeclarations");
        }
    }
}