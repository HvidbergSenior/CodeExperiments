using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class ShippingAgentCode : ValueObject
{
    public string Value { get; private set; }

    private ShippingAgentCode()
    {
        Value = "";
    }

    private ShippingAgentCode(string value)
    {
        Value = value;
    }

    public static ShippingAgentCode Create(string value)
    {
        return new ShippingAgentCode(value);
    }

    public static ShippingAgentCode Empty()
    {
        return new ShippingAgentCode();
    }
}