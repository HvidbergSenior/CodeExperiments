using Insight.BuildingBlocks.Application.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.PublishAllocations
{
    public static class PublishAllocationsEndpoint
    {
        public static void MapPublishAllocationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(AllocationEngineEndpointUrls.PUBLISH_ALLOCATIONS_ENDPOINT, async (
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var command = PublishAllocationsCommand.Create(); 
                     
                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                    })
                .RequireAuthorization()
                .WithName("PublishAllocations")
                .WithTags("Allocations");
        }
      
    }
}
