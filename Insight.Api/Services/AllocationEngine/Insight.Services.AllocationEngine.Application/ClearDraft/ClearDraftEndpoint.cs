using Insight.BuildingBlocks.Application.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Insight.Services.AllocationEngine.Application.ClearDraft
{
    public static class ClearDraftEndpoint
    {
        public static void MapClearAllocationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(
                    AllocationEngineEndpointUrls
                        .CLEAR_ALLOCATIONS_ENDPOINT,
                    async (
                        ICommandBus commandBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var command = ClearDraftCommand.Create();
                        await commandBus.Send(command, cancellationToken);
                        return Results.Ok();
                    })
                .RequireAuthorization()
                .WithName("ClearAllocations")
                .WithTags("Allocations");
        }
    }
}
