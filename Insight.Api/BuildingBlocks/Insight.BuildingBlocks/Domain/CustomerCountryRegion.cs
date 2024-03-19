namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerCountryRegion : ValueObject
{
    public string Value { get; private set; }

    private CustomerCountryRegion()
    {
        Value = "";
    }

    private CustomerCountryRegion(string value)
    {
        Value = value;
    }

    public static CustomerCountryRegion Create(string value)
    {
        return new CustomerCountryRegion(value);
    }

    public static CustomerCountryRegion Empty()
    {
        return new CustomerCountryRegion();
    }
}