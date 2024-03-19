using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class Blocked : ValueObject
{
    public string Value { get; private set; }

    private Blocked()
    {
        Value = "";
    }

    private Blocked(string value)
    {
        Value = value;
    }

    public static Blocked Create(string value)
    {
        return new Blocked(value);
    }

    public static Blocked Empty()
    {
        return new Blocked();
    }
}