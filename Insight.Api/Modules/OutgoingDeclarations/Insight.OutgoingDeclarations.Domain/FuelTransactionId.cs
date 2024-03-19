using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class FuelTransactionId : ValueObject
    {
        public Guid Value { get; private set; }

        private FuelTransactionId()
        {
            Value = default;
        }

        private FuelTransactionId(Guid value)
        {
            Value = value;
        }

        public static FuelTransactionId Create(Guid value)
        {
            return new FuelTransactionId(value);
        }

        public static FuelTransactionId Empty()
        {
            return new FuelTransactionId();
        }
    }
}