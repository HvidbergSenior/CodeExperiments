using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;

namespace Insight.Customers.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>, IReadonlyRepository<Customer>
    {
        Task<Customer?> GetByCustomerNumberAndCompanyIdAsync(CustomerNumber customerNumber,
            CompanyId companyId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeOrderedByNameAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, bool isOrderDescending);

        Task<IEnumerable<Customer>> GetCustomersByIdsAsync(IEnumerable<CustomerId> customerIds, CancellationToken cancellationToken);
    }
}