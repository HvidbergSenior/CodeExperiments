namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerBillToName : ValueObject
{
    public string Value { get; private set; }

    private CustomerBillToName()
    {
        Value = "";
    }

    private CustomerBillToName(string value)
    {
        Value = value;
    }

    public static CustomerBillToName Create(string value)
    {
        return new CustomerBillToName(value);
    }

    public static CustomerBillToName Empty()
    {
        return new CustomerBillToName();
    }
}