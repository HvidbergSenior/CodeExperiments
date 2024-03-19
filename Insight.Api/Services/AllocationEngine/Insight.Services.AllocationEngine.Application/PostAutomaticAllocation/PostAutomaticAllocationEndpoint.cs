using Insight.BuildingBlocks.Application.Commands;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.PostAutomaticAllocation
{
    public static class PostAutomaticAllocationEndpoint
    {
        public static void MapPostAutomaticAllocationEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(AllocationEngineEndpointUrls.POST_AUTOMATIC_ALLOCATION_ENDPOINT, async (
                    PostAutomaticAllocationRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var command = PostAutomaticAllocationCommand.Create(AutoAllocationFilteringParameters.Create(request.StartDate, request.EndDate, FilterProductName.Create(request.Product), FilterCompanyName.Create(request.Company), FilterCustomerName.Create(request.Customer)));
                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()
                .WithName("PostAutomaticAllocation")
                .WithTags("Allocations");
        }
    }
}
