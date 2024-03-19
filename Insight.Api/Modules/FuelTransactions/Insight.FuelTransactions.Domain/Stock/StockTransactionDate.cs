using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.Stock
{
    public sealed class StockTransactionDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private StockTransactionDate()
        {
            Value = DateOnly.MinValue;
        }

        private StockTransactionDate(DateOnly value)
        {
            Value = value;
        }

        public static StockTransactionDate Create(DateOnly value)
        {
            return new StockTransactionDate(value);
        }

        public static StockTransactionDate Empty()
        {
            return new StockTransactionDate();
        }

        public static StockTransactionDate Today()
        {
            return new StockTransactionDate(DateOnly.FromDateTime(DateTime.Today));
        }
    }

}
