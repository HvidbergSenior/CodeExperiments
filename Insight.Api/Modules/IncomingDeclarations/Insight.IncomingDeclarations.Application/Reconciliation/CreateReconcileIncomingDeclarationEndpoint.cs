using Insight.BuildingBlocks.Application.Commands;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.Reconciliation
{
    public static class CreateReconcileIncomingDeclarationEndpoint
    {
        public static void MapReconcileIncomingDeclarationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(IncomingDeclarationsEndpointUrls.RECONCILE_INCOMING_DECLARATION_ENDPOINT, async (ReconcileIncomingDeclarationsRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var incomingDeclarationIds = request.IncomingDeclarationIds.Select(IncomingDeclarationId.Create).ToArray();

                    var command = ReconcileIncomingDeclarationsCommand.Create(incomingDeclarationIds);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()
                .WithName("ReconcileIncomingDeclaration")
                .WithTags("IncomingDeclarations");
        }
    }
}
