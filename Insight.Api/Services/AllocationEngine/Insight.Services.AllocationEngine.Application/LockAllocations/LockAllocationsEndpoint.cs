using Insight.BuildingBlocks.Application.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.LockAllocations
{
    public static class LockAllocationsEndpoint
    {
        public static void MapLockAllocationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(
                    AllocationEngineEndpointUrls
                        .LOCK_ALLOCATIONS_ENDPOINT,
                    async (
                        ICommandBus commandBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var command = LockAllocationsCommand.Create();
                        await commandBus.Send(command, cancellationToken);
                        return Results.Ok();
                    })
                .RequireAuthorization()
                .WithName("LockAllocations")
                .WithTags("Allocations");
        }
    }
}
