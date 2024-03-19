using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class ShipmentMethodCode : ValueObject
{
    public string Value { get; private set; }

    private ShipmentMethodCode()
    {
        Value = "";
    }

    private ShipmentMethodCode(string value)
    {
        Value = value;
    }

    public static ShipmentMethodCode Create(string value)
    {
        return new ShipmentMethodCode(value);
    }

    public static ShipmentMethodCode Empty()
    {
        return new ShipmentMethodCode();
    }
}