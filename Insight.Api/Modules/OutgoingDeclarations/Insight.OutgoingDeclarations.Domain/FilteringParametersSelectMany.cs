namespace Insight.OutgoingDeclarations.Domain;

using Insight.BuildingBlocks.Domain;

public sealed class FilteringParametersSelectMany : ValueObject
{
    public DatePeriod DatePeriod { get; private set; } = DatePeriod.Empty();
    public IEnumerable<ProductName> ProductNames { get; private set; }
    public IEnumerable<CustomerNumber> CustomerNumbers { get; private set; }

    private FilteringParametersSelectMany()
    {
        ProductNames = new List<ProductName>();
        CustomerNumbers = new List<CustomerNumber>();
    }

    private FilteringParametersSelectMany(DatePeriod datePeriod, IEnumerable<ProductName> productNames, IEnumerable<CustomerNumber> customerNumbers)
    {
        DatePeriod = datePeriod;
        ProductNames = productNames;
        CustomerNumbers = customerNumbers;
    }

    public static FilteringParametersSelectMany Create(DatePeriod datePeriod, IEnumerable<ProductName> productNames, IEnumerable<CustomerNumber> customerNumbers)
    {
        return new FilteringParametersSelectMany(datePeriod, productNames, customerNumbers);
    }

    public static FilteringParametersSelectMany Empty()
    {
        return new FilteringParametersSelectMany();
    }
}