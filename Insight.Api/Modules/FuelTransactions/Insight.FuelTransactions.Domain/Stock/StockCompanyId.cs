using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.Stock
{
    public sealed class StockCompanyId : ValueObject
    {
        public Guid Value { get; private set; }

        private StockCompanyId()
        {
            Value = Guid.Empty;
        }

        private StockCompanyId(Guid value)
        {
            Value = value;
        }

        public static StockCompanyId Create(Guid value)
        {
            return new StockCompanyId(value);
        }

        public static StockCompanyId Empty()
        {
            return new StockCompanyId();
        }
    }

}
