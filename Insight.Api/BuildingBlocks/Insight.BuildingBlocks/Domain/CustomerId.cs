namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerId : ValueObject
{
    public Guid Value { get; private set; }

    private CustomerId()
    {
        Value = Guid.NewGuid();
    }

    private CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId Create(Guid company)
    {
        return new CustomerId(company);
    }

    public static CustomerId Empty()
    {
        return new CustomerId();
    }
}