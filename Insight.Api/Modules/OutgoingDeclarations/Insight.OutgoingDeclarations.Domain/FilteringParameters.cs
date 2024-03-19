using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class FilteringParameters : ValueObject
    {
        public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
        public Product Product { get; private set; } = Product.Empty();
        public Company Company { get; private set; } = Company.Empty();
        public CustomerName CustomerName { get; private set; } = CustomerName.Empty();

        private FilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private FilteringParameters(DatePeriod datePeriod, Product product, Company company, CustomerName customerName)
        {
            DatePeriod = datePeriod;
            Product = product;
            Company = company;
            CustomerName = customerName;
        }

        public static FilteringParameters Create(DatePeriod datePeriod, Product product, Company company, CustomerName customerName)
        {
            return new FilteringParameters(datePeriod, product, company, customerName);
        }

        public static FilteringParameters Empty()
        {
            return new FilteringParameters();
        }
    }
}
