using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class AllocationId : ValueObject
    {
        public Guid Value { get; private set; }

        private AllocationId()
        {
            Value = Guid.Empty;
        }

        private AllocationId(Guid value)
        {
            Value = value;
        }

        public static AllocationId Create(Guid value)
        {
            return new AllocationId(value);
        }

        public static AllocationId Empty()
        {
            return new AllocationId();
        }
    }
}