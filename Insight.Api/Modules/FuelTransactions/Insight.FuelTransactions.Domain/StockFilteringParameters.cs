using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class StockFilteringParameters : ValueObject
    {
        public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();

        private StockFilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private StockFilteringParameters(DatePeriod datePeriod, ProductName productName, CompanyName companyName)
        {
            DatePeriod = datePeriod;
            ProductName = productName;            
            CompanyName = companyName;
        }

        public static StockFilteringParameters Create(DatePeriod datePeriod, ProductName productName, CompanyName companyName)
        {
            return new StockFilteringParameters(datePeriod, productName, companyName);
        }

        public static StockFilteringParameters Empty()
        {
            return new StockFilteringParameters();
        }
    }
}
