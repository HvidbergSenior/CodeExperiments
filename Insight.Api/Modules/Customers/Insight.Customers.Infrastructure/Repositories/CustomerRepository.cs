using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Marten;
using Marten.Pagination;

namespace Insight.Customers.Infrastructure.Repositories
{
    public class CustomerRepository : MartenDocumentRepository<Customer>, ICustomerRepository
    {
        private readonly IDocumentSession documentSession;

        public CustomerRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) :
            base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize,
            CancellationToken cancellationToken)
        {
            return await Query().ToPagedListAsync(pageNumber == 0 ? 1 : pageNumber, pageSize, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByPageNumberAndPageSizeOrderedByNameAsync(int pageNumber, int pageSize,
            CancellationToken cancellationToken, bool isOrderDescending)
        {
            if (isOrderDescending)
            {
                return await Query().OrderByDescending(x => x.CustomerDetails.CustomerName.Value)
                    .ToPagedListAsync(pageNumber == 0 ? 1 : pageNumber, pageSize, cancellationToken);
            }
            else
            {
                return await Query().OrderBy(x => x.CustomerDetails.CustomerName.Value)
                    .ToPagedListAsync(pageNumber == 0 ? 1 : pageNumber, pageSize, cancellationToken);
            }
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByIdsAsync(IEnumerable<CustomerId> customerIds, CancellationToken cancellationToken)
        {
            return await documentSession.LoadManyAsync<Customer>(cancellationToken, customerIds.Select(c => c.Value));
        }
        
        public async Task<Customer?> GetByCustomerNumberAndCompanyIdAsync(
            CustomerNumber customerNumber, CompanyId companyId, CancellationToken cancellationToken = default)
        {
            return await Query()
                .FirstOrDefaultAsync(
                    c => c.CustomerDetails.CustomerNumber.Value == customerNumber.Value &&
                         c.CompanyId.Value == companyId.Value, cancellationToken);
        }
    }
}