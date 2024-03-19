using System.ComponentModel.DataAnnotations;

namespace Insight.Services.AllocationEngine.Application.GetAllocations
{
    public sealed class GetAllocationsResponse
    {
        [Required]
        public bool HasMoreAllocations { get; set; }
        [Required]
        public decimal TotalAmountOfAllocations { get; set; }
        [Required]
        public IReadOnlyList<AllocationResponse> Allocations { get; private set; }

        [Required]
        public bool IsDraftLocked { get; private set; }

        public GetAllocationsResponse(
            IReadOnlyList<AllocationResponse> allocations,
            bool hasMoreAllocations,
            decimal totalAmountOfAllocations, bool isDraftLocked)
        {
            Allocations = allocations;
            TotalAmountOfAllocations = totalAmountOfAllocations;
            HasMoreAllocations = hasMoreAllocations;
            IsDraftLocked = isDraftLocked;
        }
    }

    public sealed class AllocationResponse
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string PosNumber { get; set; }
        [Required]
        public required string CustomerNumber { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string Product { get; set; }
        [Required]
        public required string Customer { get; set; }
        [Required]
        public required string Storage { get; set; }
        [Required]
        public required string CertificationSystem { get; set; }
        [Required]
        public required string RawMaterial { get; set; }
        [Required]
        public required string CountryOfOrigin { get; set; }
        [Required]
        public required decimal GHGReduction { get; set; }
        [Required]
        public required decimal Volume { get; set; }
        [Required]
        public required string[] Warnings { get; set; } = Array.Empty<string>();
        [Required]
        public bool HasWarnings => Warnings.Length != 0;
        [Required]
        public required decimal FossilFuelComparatorgCO2EqPerMJ { get; set; }
    }
}
