using Insight.BuildingBlocks.Application.Commands;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId
{
    public static class CancelIncomingDeclarationsByUploadIdEndpoint
    {
        public static void MapCancelIncomingDeclarationsByUploadIdEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost(IncomingDeclarationsEndpointUrls.CANCEL_INCOMING_DECLARATIONS_BY_UPLOAD_ID_ENDPOINT, async (
                    CancelIncomingDeclarationsByUploadIdRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var uploadId = IncomingDeclarationUploadId.Create(request.IncomingDeclarationUploadId);
                    var command = CancelIncomingDeclarationsByUploadIdCommand.Create(uploadId);
                    await commandBus.Send(command, cancellationToken);
                    return Results.Ok();
                })
                .RequireAuthorization()
                .WithName("CancelIncomingDeclarationByUploadId")
                .WithTags("IncomingDeclarations");
        }
    }
}
