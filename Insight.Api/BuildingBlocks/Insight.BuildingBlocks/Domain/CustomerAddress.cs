namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerAddress : ValueObject
{
    public string Value { get; private set; }

    private CustomerAddress()
    {
        Value = "";
    }

    private CustomerAddress(string value)
    {
        Value = value;
    }

    public static CustomerAddress Create(string value)
    {
        return new CustomerAddress(value);
    }

    public static CustomerAddress Empty()
    {
        return new CustomerAddress();
    }
}