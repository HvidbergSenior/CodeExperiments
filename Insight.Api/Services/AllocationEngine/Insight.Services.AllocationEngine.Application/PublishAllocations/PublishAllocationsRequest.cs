namespace Insight.Services.AllocationEngine.Application.PublishAllocations
{
    public sealed class PublishAllocationsRequest
    {
        public required FuelTransactionsBatch FuelTransactionsBatch { get; set; }
        public required Allocation[] Allocations { get; set; }
    }

    public sealed class FuelTransactionsBatch
    {
        public required Guid CustomerId { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required string ProductNumber { get; set; }
        public required string Country { get; set; }
        public required string StationName { get; set; }
        public required string LocationId { get; set; }
    }

    public sealed class Allocation
    {
        public Guid IncomingDeclarationId { get; set; }
        public decimal Volume { get; set; }
    }
}