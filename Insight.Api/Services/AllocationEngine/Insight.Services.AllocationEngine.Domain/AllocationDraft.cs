using Insight.BuildingBlocks.Domain;
using Insight.Services.AllocationEngine.Domain.Converters;
using Newtonsoft.Json;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class AllocationDraft : Entity
    {
        public AllocationDraftId AllocationDraftId { get; private set; } = AllocationDraftId.Instance;
        public AllocationDraftState AllocationDraftState { get; private set; } = AllocationDraftState.Unlocked;
        [JsonConverter(typeof(AllocationCollectionConverter))]
        public AllocationCollection Allocations { get; private set; } = new AllocationCollection();

        public TemporaryAllocationId TemporaryAllocationId { get; private set; } = TemporaryAllocationId.Empty();

        private AllocationDraft()
        {
            Id = AllocationDraftId.Value;
            TemporaryAllocationId = TemporaryAllocationId.Create(Guid.NewGuid());
        }

        public bool AddAllocation(Allocation allocation)
        {
            if(AllocationDraftState == AllocationDraftState.Locked)
            {
                return false;
            }

            if(allocation.FuelTransactionIds.Select(c=> c.Value).Intersect(FuelTransactionIds).Any())
            {
                return false; // Todo: Throw exception.
            }

            return Allocations.TryAdd(allocation.AllocationId, allocation);
        }

        public bool RemoveAllocation(AllocationId allocationId)
        {
            if (AllocationDraftState == AllocationDraftState.Locked)
            {
                return false;
            }
            return Allocations.Remove(allocationId);
        }
        public bool IsLocked => AllocationDraftState == AllocationDraftState.Locked;

        public void Lock()
        {
            AllocationDraftState = AllocationDraftState.Locked;
        }

        public void Unlock()
        {
            AllocationDraftState = AllocationDraftState.Unlocked;
        }

        public static AllocationDraft Create()
        {
            return new AllocationDraft();
        }

        public bool AllowClear => AllocationDraftState == AllocationDraftState.Unlocked;

        // Used for Marten indexing
        public Guid[] FuelTransactionIds => Allocations.SelectMany(c => c.Value.FuelTransactionIds).Select(c => c.Value).ToArray();
    }
}
