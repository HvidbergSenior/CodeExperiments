using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.OutgoingDeclarations.Domain;
using Marten;
using Marten.Pagination;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.OutgoingDeclarations.Infrastructure
{
    public class OutgoingDeclarationRepository : MartenDocumentRepository<OutgoingDeclaration>, IOutgoingDeclarationRepository
    {
        private readonly IDocumentSession documentSession;

        public OutgoingDeclarationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }
        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<OutgoingDeclaration>> GetByCustomerNumberAsync(string customerNumberValue, CancellationToken cancellationToken = default)
        {
            return await Query().Where(c => c.CustomerDetails.CustomerNumber.Value == customerNumberValue).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<OutgoingDeclaration>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Query().ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<OutgoingDeclaration> Items, int TotalCount, bool HasMore)> GetOutgoingDeclarationsByPageNumberAndPageSize(int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var query = Query();

            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                query = query.Where(declaration => declaration.DateOfCreation.Value >= filteringParameters.DatePeriod.StartDate && declaration.DateOfCreation.Value <= filteringParameters.DatePeriod.EndDate);
            }
            
            if (!filteringParameters.Product.Value.ToUpperInvariant().Equals(Product.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Product.Value.ToUpperInvariant().Contains(filteringParameters.Product.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            if (!filteringParameters.CustomerName.Value.ToUpperInvariant().Equals(CustomerName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.CustomerDetails.CustomerName.Value.ToUpperInvariant().Contains(filteringParameters.CustomerName.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }
            
            query = sortingParameters.IsOrderDescending
                ? query.OrderByDescending(sortingParameters.OrderByProperty.FirstCharToUpper())
                : query.OrderBy(sortingParameters.OrderByProperty.FirstCharToUpper());

            var pagedList = await query.ToPagedListAsync(pageNumber, pageSize, cancellationToken);
            
            return (pagedList, Convert.ToInt32(pagedList.TotalItemCount), pagedList.HasNextPage);
        }

        public async Task<IEnumerable<OutgoingDeclaration>> GetOutgoingDeclarationsForMany(FilteringParametersSelectMany filteringParameters, CancellationToken cancellationToken = default)
        {
            var query = Query();

            if (query.Any())
            {
                if (filteringParameters.DatePeriod != DatePeriod.Empty())
                {
                    query = query.Where(declaration =>
                        declaration.DateOfCreation.Value >= filteringParameters.DatePeriod.StartDate &&
                        declaration.DateOfCreation.Value <= filteringParameters.DatePeriod.EndDate);
                }
                
                if (query.Any() && filteringParameters.ProductNames.Any())
                {
                    var productNamesValuesFilter = filteringParameters.ProductNames.Select(c => c.Value.ToUpperInvariant()).ToList();
                    query = query.Where(d => productNamesValuesFilter.Contains(d.Product.Value.ToUpperInvariant()));
            
                }
                
                if (query.Any() && filteringParameters.CustomerNumbers.Any())
                {
                    var customerNumbersValuesFilter = filteringParameters.CustomerNumbers.Select(c => c.Value).ToList();
                    query = query.Where(d => customerNumbersValuesFilter.Contains(d.CustomerDetails.CustomerNumber.Value));
                }
            }

            var results = await query.ToListAsync(cancellationToken);

            return results;
        }
    }
}