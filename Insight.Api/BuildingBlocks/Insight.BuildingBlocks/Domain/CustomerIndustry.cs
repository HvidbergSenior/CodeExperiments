namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerIndustry : ValueObject
{
    public string Value { get; private set; }

    private CustomerIndustry()
    {
        Value = "";
    }

    private CustomerIndustry(string value)
    {
        Value = value;
    }

    public static CustomerIndustry Create(string value)
    {
        return new CustomerIndustry(value);
    }

    public static CustomerIndustry Empty()
    {
        return new CustomerIndustry();
    }
}