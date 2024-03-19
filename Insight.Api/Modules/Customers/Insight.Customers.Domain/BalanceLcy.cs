using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class BalanceLcy : ValueObject
{
    public decimal Value { get; private set; }

    private BalanceLcy()
    {
        Value = default;
    }

    private BalanceLcy(decimal value)
    {
        Value = value;
    }

    public static BalanceLcy Create(decimal value)
    {
        return new BalanceLcy(value);
    }

    public static BalanceLcy Empty()
    {
        return new BalanceLcy();
    }
}