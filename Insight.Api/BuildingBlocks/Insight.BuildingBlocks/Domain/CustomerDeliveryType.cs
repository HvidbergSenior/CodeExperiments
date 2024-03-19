namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerDeliveryType : ValueObject
{
    public string Value { get; private set; }

    private CustomerDeliveryType()
    {
        Value = "";
    }

    private CustomerDeliveryType(string value)
    {
        Value = value;
    }

    public static CustomerDeliveryType Create(string value)
    {
        return new CustomerDeliveryType(value);
    }

    public static CustomerDeliveryType Empty()
    {
        return new CustomerDeliveryType();
    }
}