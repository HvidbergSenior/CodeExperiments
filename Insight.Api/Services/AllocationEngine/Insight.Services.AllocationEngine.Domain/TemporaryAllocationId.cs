using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class TemporaryAllocationId : ValueObject
    {
        public Guid Value { get; private set; }

        private TemporaryAllocationId()
        {
            Value = Guid.Empty;
        }

        private TemporaryAllocationId(Guid value)
        {
            Value = value;
        }

        public static TemporaryAllocationId Create(Guid value)
        {
            return new TemporaryAllocationId(value);
        }

        public static TemporaryAllocationId Empty()
        {
            return new TemporaryAllocationId();
        }
    }
}