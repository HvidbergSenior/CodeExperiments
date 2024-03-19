using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionId : ValueObject
    {
        public Guid Value { get; private set; }

        private FuelTransactionId()
        {
            Value = Guid.Empty;
        }

        private FuelTransactionId(Guid value)
        {
            Value = value;
        }

        public static FuelTransactionId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new FuelTransactionId(value);
        }

        public static FuelTransactionId Empty()
        {
            return new FuelTransactionId(Guid.Empty);
        }
    }
}
