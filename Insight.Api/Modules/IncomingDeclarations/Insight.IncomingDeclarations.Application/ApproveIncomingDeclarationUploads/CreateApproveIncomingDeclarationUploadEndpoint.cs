using Insight.BuildingBlocks.Application.Commands;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads
{
    public static class CreateApproveIncomingDeclarationUploadEndpoint
    {
        public static void MapApproveIncomingDeclarationUploadEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(IncomingDeclarationsEndpointUrls.APPROVE_INCOMING_DECLARATION_UPLOAD_ENDPOINT, async (
                    ApproveIncomingDeclarationUploadRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var uploadId = IncomingDeclarationUploadId.Create(request.IncomingDeclarationUploadId);

                     var command = ApproveIncomingDeclarationUploadCommand.Create(uploadId); 
                     
                     await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()
                .WithName("ApproveIncomingDeclarationUpload")
                .WithTags("IncomingDeclarations");
        }
    }
}
