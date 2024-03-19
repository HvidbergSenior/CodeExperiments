namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerCity : ValueObject
{
    public string Value { get; private set; }

    private CustomerCity()
    {
        Value = "";
    }

    private CustomerCity(string value)
    {
        Value = value;
    }

    public static CustomerCity Create(string value)
    {
        return new CustomerCity(value);
    }

    public static CustomerCity Empty()
    {
        return new CustomerCity();
    }
}