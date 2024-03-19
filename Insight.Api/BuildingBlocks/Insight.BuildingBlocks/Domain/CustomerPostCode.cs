namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerPostCode : ValueObject
{
    public string Value { get; private set; }

    private CustomerPostCode()
    {
        Value = "";
    }

    private CustomerPostCode(string value)
    {
        Value = value;
    }

    public static CustomerPostCode Create(string value)
    {
        return new CustomerPostCode(value);
    }

    public static CustomerPostCode Empty()
    {
        return new CustomerPostCode();
    }
}