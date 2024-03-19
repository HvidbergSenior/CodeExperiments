using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class VatRegNumber : ValueObject
{
    public string Value { get; private set; }

    private VatRegNumber()
    {
        Value = "";
    }

    private VatRegNumber(string value)
    {
        Value = value;
    }

    public static VatRegNumber Create(string value)
    {
        return new VatRegNumber(value);
    }

    public static VatRegNumber Empty()
    {
        return new VatRegNumber();
    }
}