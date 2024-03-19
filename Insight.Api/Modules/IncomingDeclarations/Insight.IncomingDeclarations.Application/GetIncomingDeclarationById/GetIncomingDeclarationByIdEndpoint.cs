using Insight.BuildingBlocks.Application.Queries;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationById
{
    public static class GetIncomingDeclarationByIdEndpoint
    {
        public static void MapGetIncomingDeclarationByIdEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    IncomingDeclarationsEndpointUrls
                        .GET_INCOMING_DECLARATION_BY_ID_ENDPOINT,
                    async (
                        Guid id,
                        IQueryBus queryBus, CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetIncomingDeclarationByIdQuery.Create(IncomingDeclarationId.Create(id));

                        var results = await queryBus
                            .Send<GetIncomingDeclarationByIdQuery,
                                IncomingDeclarationDto>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<IncomingDeclarationDto>()
                .WithName("GetIncomingDeclarationById")
                .WithTags("IncomingDeclarations");
        }
    }
}