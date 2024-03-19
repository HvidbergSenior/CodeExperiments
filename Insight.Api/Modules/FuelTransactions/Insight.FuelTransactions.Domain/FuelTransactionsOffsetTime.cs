using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionsOffsetTime : ValueObject
    {
        public DateTimeOffset Value { get; private set; }

        private FuelTransactionsOffsetTime()
        {
            Value = DateTimeOffset.MinValue;
        }

        private FuelTransactionsOffsetTime(DateTimeOffset value)
        {
            Value = value;
        }

        public static FuelTransactionsOffsetTime Create(DateTimeOffset value)
        {
            return new FuelTransactionsOffsetTime(value);
        }

        public static FuelTransactionsOffsetTime Empty()
        {
            return new FuelTransactionsOffsetTime();
        }
    }
}
