using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class BalanceDueLcy : ValueObject
{
    public decimal Value { get; private set; }

    private BalanceDueLcy()
    {
        Value = default;
    }

    private BalanceDueLcy(decimal value)
    {
        Value = value;
    }

    public static BalanceDueLcy Create(decimal value)
    {
        return new BalanceDueLcy(value);
    }

    public static BalanceDueLcy Empty()
    {
        return new BalanceDueLcy();
    }
}