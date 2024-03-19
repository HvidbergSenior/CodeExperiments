using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class FilteringParameters : ValueObject
    {
        public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
        public Product Product { get; private set; } = Product.Empty();
        public Company Company { get; private set; } = Company.Empty();
        public Supplier Supplier { get; private set; } = Supplier.Empty();

        private FilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private FilteringParameters(DatePeriod datePeriod, Product product, Company company, Supplier supplier)
        {
            DatePeriod = datePeriod;
            Product = product;
            Company = company;
            Supplier = supplier;
        }

        public static FilteringParameters Create(DatePeriod datePeriod, Product product, Company company, Supplier supplier)
        {
            return new FilteringParameters(datePeriod, product, company, supplier);
        }

        public static FilteringParameters Empty()
        {
            return new FilteringParameters();
        }
    }
}
