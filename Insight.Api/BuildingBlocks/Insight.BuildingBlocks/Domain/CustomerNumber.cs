using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerNumber : ValueObject
{
    public string Value { get; private set; }

    private CustomerNumber()
    {
        Value = "";
    }

    private CustomerNumber(string value)
    {
        Value = value;
    }

    public static CustomerNumber Create(string value)
    {
        return new CustomerNumber(value);
    }

    public static CustomerNumber Empty()
    {
        return new CustomerNumber();
    }
}