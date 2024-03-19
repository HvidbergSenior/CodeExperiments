using Insight.BuildingBlocks.Application.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration;

public static class UploadIncomingDeclarationEndpoint
{
    public static void MapUploadIncomingDeclarationEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint
            .MapPost(IncomingDeclarationsEndpointUrls.UPLOAD_INCOMING_DECLARATION_ENDPOINT, async (UploadIncomingDeclarationRequest request, ICommandBus commandBus, CancellationToken cancellationToken) =>
            {
                var command = UploadIncomingDeclarationCommand.Create(request.ExcelFile, request.IncomingDeclarationSupplier);
                var result = await commandBus.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .Produces<UploadIncomingDeclarationCommandResponse>()
            .WithName("UploadIncomingDeclaration")
            .WithTags("IncomingDeclarations");
    }
}