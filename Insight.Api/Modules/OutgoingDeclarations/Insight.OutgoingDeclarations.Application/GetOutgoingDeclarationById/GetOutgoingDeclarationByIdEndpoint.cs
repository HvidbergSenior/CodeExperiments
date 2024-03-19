using Insight.BuildingBlocks.Application.Queries;
using Insight.OutgoingDeclarations.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById
{
    public static class GetOutgoingDeclarationByIdEndpoint
    {
        public static void MapGetOutgoingDeclarationByIdEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATION_BY_ID_ENDPOINT,
                    async (
                        Guid id,
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetOutgoingDeclarationByIdQuery.Create(OutgoingDeclarationId.Create(id));

                        var results = await queryBus
                            .Send<GetOutgoingDeclarationByIdQuery,
                                GetOutgoingDeclarationByIdResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetOutgoingDeclarationByIdResponse>()
                .WithName("GetOutgoingDeclarationById")
                .WithTags("OutgoingDeclarations");
        }
    }
}