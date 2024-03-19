using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class OutstandingOrdersLcy : ValueObject
{
    public decimal Value { get; private set; }

    private OutstandingOrdersLcy()
    {
        Value = default;
    }

    private OutstandingOrdersLcy(decimal value)
    {
        Value = value;
    }

    public static OutstandingOrdersLcy Create(decimal value)
    {
        return new OutstandingOrdersLcy(value);
    }

    public static OutstandingOrdersLcy Empty()
    {
        return new OutstandingOrdersLcy();
    }
}