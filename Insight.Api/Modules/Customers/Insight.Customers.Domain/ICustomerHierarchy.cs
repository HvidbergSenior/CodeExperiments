namespace Insight.Customers.Domain;

public interface ICustomerHierarchy
{
    Task<IReadOnlyList<CustomerNode>> GetCustomerNodes(CancellationToken cancellationToken);
    void ClearCache();
}