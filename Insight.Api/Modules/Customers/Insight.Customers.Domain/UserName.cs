using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class UserName : ValueObject
{
    public string Value { get; private set; }

    private UserName()
    {
        Value = "";
    }

    private UserName(string value)
    {
        Value = value;
    }

    public static UserName Create(string value)
    {
        return new UserName(value);
    }

    public static UserName Empty()
    {
        return new UserName();
    }
}