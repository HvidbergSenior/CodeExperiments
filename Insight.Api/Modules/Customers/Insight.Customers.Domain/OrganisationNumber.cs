using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class OrganisationNumber : ValueObject
{
    public string Value { get; private set; }

    private OrganisationNumber()
    {
        Value = "";
    }

    private OrganisationNumber(string value)
    {
        Value = value;
    }

    public static OrganisationNumber Create(string value)
    {
        return new OrganisationNumber(value);
    }

    public static OrganisationNumber Empty()
    {
        return new OrganisationNumber();
    }
}