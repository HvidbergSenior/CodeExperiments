using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class SalesPerson : ValueObject
{
    public string Value { get; private set; }

    private SalesPerson()
    {
        Value = "";
    }

    private SalesPerson(string value)
    {
        Value = value;
    }

    public static SalesPerson Create(string value)
    {
        return new SalesPerson(value);
    }

    public static SalesPerson Empty()
    {
        return new SalesPerson();
    }
}