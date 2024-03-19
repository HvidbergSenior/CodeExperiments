namespace Insight.BuildingBlocks.Domain
{
    public sealed class SustainabilityAndFuelConsumptionFilteringParameters : ValueObject
    {
        public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
        public IEnumerable<ProductName> ProductNames { get; private set; }
        public IEnumerable<CustomerId> CustomerIds { get; private set; }
        public IEnumerable<CustomerNumber> CustomerNumbers { get; private set; }
        public MaxColumns MaxColumns { get; private set; } = MaxColumns.Empty();

        private SustainabilityAndFuelConsumptionFilteringParameters()
        {
            ProductNames = new List<ProductName>();
            CustomerIds = new List<CustomerId>();
            CustomerNumbers = new List<CustomerNumber>();
        }

        private SustainabilityAndFuelConsumptionFilteringParameters(DatePeriod datePeriod, IEnumerable<ProductName> productNames, IEnumerable<CustomerId> customerIds, IEnumerable<CustomerNumber> customerNumbers, MaxColumns maxColumns)
        {
            DatePeriod = datePeriod;
            ProductNames = productNames;
            CustomerIds = customerIds;
            CustomerNumbers = customerNumbers;
            MaxColumns = maxColumns;
        }

        public static SustainabilityAndFuelConsumptionFilteringParameters Create(DatePeriod datePeriod, IEnumerable<ProductName> productNames, IEnumerable<CustomerId> customerIds, IEnumerable<CustomerNumber> customerNumbers, MaxColumns maxColumns)
        {
            return new SustainabilityAndFuelConsumptionFilteringParameters(datePeriod, productNames, customerIds, customerNumbers, maxColumns);
        }

        public static SustainabilityAndFuelConsumptionFilteringParameters Empty()
        {
            return new SustainabilityAndFuelConsumptionFilteringParameters();
        }
    }
}
