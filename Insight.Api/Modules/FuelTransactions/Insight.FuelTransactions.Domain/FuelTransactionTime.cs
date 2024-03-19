using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionTime : ValueObject
    {
        public string Value { get; private set; }

        private FuelTransactionTime()
        {
            Value = string.Empty;
        }

        private FuelTransactionTime(string value)
        {
            Value = value;
        }

        public static FuelTransactionTime Create(string value)
        {
            return new FuelTransactionTime(value);
        }

        public static FuelTransactionTime Empty()
        {
            return new FuelTransactionTime();
        }
    }
}
