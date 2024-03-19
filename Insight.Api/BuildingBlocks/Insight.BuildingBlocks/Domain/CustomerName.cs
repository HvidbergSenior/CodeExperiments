using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerName : ValueObject
{
    public string Value { get; private set; }

    private CustomerName()
    {
        Value = "";
    }

    private CustomerName(string value)
    {
        Value = value;
    }

    public static CustomerName Create(string value)
    {
        return new CustomerName(value);
    }

    public static CustomerName Empty()
    {
        return new CustomerName();
    }
}