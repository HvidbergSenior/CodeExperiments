using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionDate : ValueObject
    {
        public string Value { get; private set; }

        private FuelTransactionDate()
        {
            Value = string.Empty;
        }

        private FuelTransactionDate(string value)
        {
            Value = value;
        }

        public static FuelTransactionDate Create(string value)
        {
            return new FuelTransactionDate(value);
        }

        public static FuelTransactionDate Empty()
        {
            return new FuelTransactionDate();
        }
    }
}
