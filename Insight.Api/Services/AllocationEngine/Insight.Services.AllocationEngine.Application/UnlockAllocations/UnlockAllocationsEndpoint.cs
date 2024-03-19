using Insight.BuildingBlocks.Application.Commands;
using Insight.Services.AllocationEngine.Application.LockAllocations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.UnlockAllocations
{
    public static class UnlockAllocationsEndpoint
    {
        public static void MapUnlockAllocationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(
                    AllocationEngineEndpointUrls
                        .UNLOCK_ALLOCATIONS_ENDPOINT,
                    async (
                        ICommandBus commandBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var command = UnlockAllocationsCommand.Create();
                        await commandBus.Send(command, cancellationToken);
                        return Results.Ok();
                    })
                .RequireAuthorization()
                .WithName("UnlockAllocations")
                .WithTags("Allocations");
        }
    }
}
