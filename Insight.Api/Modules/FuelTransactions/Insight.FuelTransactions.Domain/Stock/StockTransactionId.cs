using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.Stock
{
    public sealed class StockTransactionId : ValueObject
    {
        public Guid Value { get; private set; }

        private StockTransactionId()
        {
            Value = Guid.Empty;
        }

        private StockTransactionId(Guid value)
        {
            Value = value;
        }

        public static StockTransactionId Create(Guid value)
        {
            return new StockTransactionId(value);
        }

        public static StockTransactionId Empty()
        {
            return new StockTransactionId();
        }
    }
}
