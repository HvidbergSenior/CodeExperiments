using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Marten;

namespace Insight.Customers.Infrastructure.Repositories
{
    public class CustomerInMemoryRepository : InMemoryRepository<Customer>, ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var skip = pageNumber * pageSize - pageSize;

            return Task.FromResult(Entities.Values.Skip(skip).Take(pageSize).Select(Deserialize));
        }

        public Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeOrderedByNameAsync(int pageNumber,
            int pageSize,
            CancellationToken cancellationToken, bool isOrderDescending)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByCustomerNumberAndCompanyIdAsync(CustomerNumber customerNumber,
            CompanyId companyId, CancellationToken cancellationToken = default)
        {
            var result = Entities.Values.Select(Deserialize)
                .FirstOrDefault(x =>
                    x.CustomerDetails.CustomerNumber.Value == customerNumber.Value &&
                    x.CompanyId.Value == companyId.Value);

            return Task.FromResult(result);
        }

        public Task<IEnumerable<Customer>> GetCustomersByIdsAsync(IEnumerable<CustomerId> customerIds, CancellationToken cancellationToken)
        {
            return Task.FromResult(Entities.Where(c => c.Key.In(customerIds.Select(o => o.Value).ToArray())).Select(c=> Deserialize(c.Value)));
        }
    }
}