using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Services.AllocationEngine.Application.PostManualAllocation
{
    public static class PostManualAllocationEndpoint
    {
        public static void MapPostManualAllocationEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(AllocationEngineEndpointUrls.POST_MANUAL_ALLOCATION_ENDPOINT, async (
                    PostManualAllocationRequest request,
                    ICommandBus commandBus,
                    CancellationToken cancellationToken) =>
                {
                    var customerId = FuelTransactionCustomerId.Create(request.FuelTransactionsBatch.CustomerId);
                    var productNumber = ProductNumber.Create(request.FuelTransactionsBatch.ProductNumber);
                    var country = FuelTransactionCountry.Create(request.FuelTransactionsBatch.Country);
                    var stationName = StationName.Create(request.FuelTransactionsBatch.StationName);
                    var productName = ProductName.Create(request.FuelTransactionsBatch.ProductName);

                    var locationId = LocationId.Create(request.FuelTransactionsBatch.LocationId);

                    var batch = FuelTransactionsBatch.Create(customerId,
                                                             request.FuelTransactionsBatch.StartDate,
                                                             request.FuelTransactionsBatch.EndDate,
                                                             productNumber,
                                                             country,
                                                             stationName,
                                                             productName,
                                                             locationId);
                    
                    var allocationAssignments = request.Allocations.Select(c => AllocationAssignment.Create(IncomingDeclarationId.Create(c.IncomingDeclarationId),
                                                                                                            Quantity.Create(c.Volume)));


                    var manualAllocation = ManualAllocationAssignments.Create(batch, allocationAssignments.ToArray());


                    var command = PostManualAllocationCommand.Create(manualAllocation);

                    await commandBus.Send(command, cancellationToken);

                    return Results.Ok();
                })
                .RequireAuthorization()
                .WithName("PostManualAllocation")
                .WithTags("Allocations");
        }
    }
}
