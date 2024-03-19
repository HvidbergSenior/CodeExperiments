namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class DapperMissingAllocationEntry
    {
        public required Guid CustomerId { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required string ProductNumber { get; set; }
        public required string Country { get; set; }
        public required string StationName { get; set; }
        public required string LocationId { get; set; }
    }
}
