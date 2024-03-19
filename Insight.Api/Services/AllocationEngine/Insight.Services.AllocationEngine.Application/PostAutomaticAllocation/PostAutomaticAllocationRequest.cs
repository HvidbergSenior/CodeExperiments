namespace Insight.Services.AllocationEngine.Application.PostAutomaticAllocation
{
    public class PostAutomaticAllocationRequest
    {
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required string Product { get; set; }
        public required string Company { get; set; }
        public required string Customer { get; set; }
    }
}
