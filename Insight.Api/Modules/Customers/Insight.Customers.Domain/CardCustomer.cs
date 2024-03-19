using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class CardCustomer : ValueObject
{
    public bool Value { get; private set; }

    private CardCustomer()
    {
        Value = default;
    }

    private CardCustomer(bool value)
    {
        Value = value;
    }

    public static CardCustomer Create(bool value)
    {
        return new CardCustomer(value);
    }

    public static CardCustomer Empty()
    {
        return new CardCustomer();
    }
}