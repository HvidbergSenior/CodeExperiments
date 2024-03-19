using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class DraftAllocationId : ValueObject
    {
        public Guid Value { get; private set; }

        private DraftAllocationId()
        {
            Value = Guid.Empty;
        }

        private DraftAllocationId(Guid value)
        {
            Value = value;
        }

        public static DraftAllocationId Create(Guid value)
        {
            return new DraftAllocationId(value);
        }

        public static DraftAllocationId Empty()
        {
            return new DraftAllocationId();
        }
    }
}