using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class CreditLimit : ValueObject
{
    public decimal Value { get; private set; }

    private CreditLimit()
    {
        Value = default;
    }

    private CreditLimit(decimal value)
    {
        Value = value;
    }

    public static CreditLimit Create(decimal value)
    {
        return new CreditLimit(value);
    }

    public static CreditLimit Empty()
    {
        return new CreditLimit();
    }
}