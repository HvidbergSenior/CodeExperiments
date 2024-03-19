using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class ManualAllocationAssignments : ValueObject
    {
        public FuelTransactionsBatch FuelTransactionsBatch { get; private set; } = FuelTransactionsBatch.Empty();
        public AllocationAssignment[] AllocationAssignments { get; private set; } = Array.Empty<AllocationAssignment>();

        private ManualAllocationAssignments(FuelTransactionsBatch fuelTransactionsBatch, AllocationAssignment[] allocationAssignments)
        {
            FuelTransactionsBatch = fuelTransactionsBatch;
            AllocationAssignments = allocationAssignments;
        }

        public static ManualAllocationAssignments Create(FuelTransactionsBatch fuelTransactionsBatch, AllocationAssignment[] allocationAssignments)
        {
            return new ManualAllocationAssignments(fuelTransactionsBatch, allocationAssignments);
        }

    }
}
