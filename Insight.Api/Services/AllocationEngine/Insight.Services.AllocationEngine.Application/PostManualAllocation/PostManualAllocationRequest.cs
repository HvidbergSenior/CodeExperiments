namespace Insight.Services.AllocationEngine.Application.PostManualAllocation
{
    public sealed class PostManualAllocationRequest
    {
        public required FuelTransactionsBatchRequest FuelTransactionsBatch { get; set; }
        public required AllocationRequest[] Allocations { get; set; }
    }

    public sealed class FuelTransactionsBatchRequest
    {
        public required Guid CustomerId { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required string ProductNumber { get; set; }
        public required string Country { get; set; }
        public required string StationName { get; set; }
        public required string ProductName { get; set; }
        public required string LocationId { get; set; }
    }

    public sealed class AllocationRequest
    {
        public Guid IncomingDeclarationId { get; set; }
        public decimal Volume { get; set; }
    }
}
