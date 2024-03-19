using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class AllocationTotal : ValueObject
    {
        public decimal Value { get; private set; }

        private AllocationTotal()
        {
            Value = default;
        }

        private AllocationTotal(decimal value)
        {
            Value = value;
        }

        public static AllocationTotal Create(decimal value)
        {
            return new AllocationTotal(value);
        }

        public static AllocationTotal Empty()
        {
            return new AllocationTotal();
        }
    }
}