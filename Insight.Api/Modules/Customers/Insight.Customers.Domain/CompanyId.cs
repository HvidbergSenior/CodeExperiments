using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public class CompanyId : ValueObject
{
    public Guid Value { get; private set; }

    private CompanyId()
    {
        Value = Guid.Empty;
    }

    private CompanyId(Guid value)
    {
        Value = value;
    }

    public static CompanyId Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid is empty", nameof(id));
        }

        return new CompanyId(id);
    }

    public static CompanyId Empty()
    {
        return new CompanyId(Guid.Empty);

    }
}