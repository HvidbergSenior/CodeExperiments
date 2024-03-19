namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerBillToNumber : ValueObject
{
    public string Value { get; private set; }

    private CustomerBillToNumber()
    {
        Value = "";
    }

    private CustomerBillToNumber(string value)
    {
        Value = value;
    }

    public static CustomerBillToNumber Create(string value)
    {
        return new CustomerBillToNumber(value);
    }

    public static CustomerBillToNumber Empty()
    {
        return new CustomerBillToNumber();
    }
}