using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionsOffsetId : ValueObject
    {
        public Guid Value { get; private set; }

        private FuelTransactionsOffsetId()
        {
            Value = Guid.Empty;
        }

        private FuelTransactionsOffsetId(Guid value)
        {
            Value = value;
        }

        public static FuelTransactionsOffsetId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new FuelTransactionsOffsetId(value);
        }

        public static FuelTransactionsOffsetId Empty()
        {
            return new FuelTransactionsOffsetId(Guid.Empty);
        }
    }
}
