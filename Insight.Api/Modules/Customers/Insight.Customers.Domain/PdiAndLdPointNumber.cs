using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class PdiAndLdPointNumber : ValueObject
{
    public string Value { get; private set; }

    private PdiAndLdPointNumber()
    {
        Value = "";
    }

    private PdiAndLdPointNumber(string value)
    {
        Value = value;
    }

    public static PdiAndLdPointNumber Create(string value)
    {
        return new PdiAndLdPointNumber(value);
    }

    public static PdiAndLdPointNumber Empty()
    {
        return new PdiAndLdPointNumber();
    }
}