using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FilteringParameters : ValueObject
    {
        public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public CustomerName CustomerName { get; private set; } = CustomerName.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();

        private FilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private FilteringParameters(DatePeriod datePeriod, ProductName productName, CustomerName customerName, CompanyName companyName)
        {
            DatePeriod = datePeriod;
            ProductName = productName;
            CustomerName = customerName;
            CompanyName = companyName;
        }

        public static FilteringParameters Create(DatePeriod datePeriod,  ProductName productName, CustomerName customerName, CompanyName companyName)
        {
            return new FilteringParameters(datePeriod, productName, customerName, companyName);
        }

        public static FilteringParameters Empty()
        {
            return new FilteringParameters();
        }
    }
}
