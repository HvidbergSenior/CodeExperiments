using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class OutgoingAllocated : ValueObject
    {
        public decimal Value { get; private set; }

        private OutgoingAllocated()
        {
            Value = decimal.Zero;
        }

        private OutgoingAllocated(decimal value)
        {
            Value = value;
        }

        public static OutgoingAllocated Create(decimal value)
        {
            return new OutgoingAllocated(value);
        }

        public static OutgoingAllocated Empty()
        {
            return new OutgoingAllocated();
        }
    }
}