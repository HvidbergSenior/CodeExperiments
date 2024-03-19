using System.Globalization;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;
using Marten.Pagination;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class IncomingDeclarationRepository : MartenDocumentRepository<IncomingDeclaration>, IIncomingDeclarationRepository
    {
        private readonly IDocumentSession documentSession;

        public IncomingDeclarationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetByIncomingDeclarationIdsAndStateAsync(IncomingDeclarationId[] incomingDeclarationIds, IncomingDeclarationState incomingDeclarationState, CancellationToken cancellationToken = default)
        {
            var ids = incomingDeclarationIds.Select(id => id.Value).ToArray();

            return await Query().Where(c => c.Id.In(ids) && c.IncomingDeclarationState == incomingDeclarationState).ToListAsync(cancellationToken);
        }
        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }

        public async Task<(IEnumerable<IncomingDeclaration> Items, int TotalCount, bool HasMore)> GetIncomingDeclarationsByPageNumberAndPageSize(int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, IEnumerable<IncomingDeclarationState> incomingDeclarationStates, CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var query = Query();

            query = query.Where(query => query.IncomingDeclarationState.In(incomingDeclarationStates.ToArray()) && query.IncomingDeclarationState != IncomingDeclarationState.Temporary);

            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                query = query.Where(declaration =>
                    declaration.DateOfDispatch.Value >= filteringParameters.DatePeriod.StartDate &&
                    declaration.DateOfDispatch.Value <= filteringParameters.DatePeriod.EndDate);
            }
            
            if (!filteringParameters.Product.Value.ToUpperInvariant().Equals(Product.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Product.Value.ToUpperInvariant().Contains(filteringParameters.Product.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            if (!filteringParameters.Company.Value.ToUpperInvariant().Equals(Company.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Company.Value.ToUpperInvariant().Contains(filteringParameters.Company.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            if (!filteringParameters.Supplier.Value.ToUpperInvariant().Equals(Supplier.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Supplier.Value.ToUpperInvariant().Contains(filteringParameters.Supplier.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            query = sortingParameters.IsOrderDescending
                ? query.OrderByDescending(sortingParameters.OrderByProperty.FirstCharToUpper())
                : query.OrderBy(sortingParameters.OrderByProperty.FirstCharToUpper());

            var pagedList = await query.ToPagedListAsync(pageNumber, pageSize, cancellationToken);

            return (pagedList, Convert.ToInt32(pagedList.TotalItemCount), pagedList.HasNextPage);
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetTemporaryIncomingDeclarationsByUploadIdAsync(
            IncomingDeclarationUploadId incomingDeclarationUploadId, CancellationToken cancellationToken = default)
        {
            return await Query().Where(c =>
                    c.IncomingDeclarationUploadId.Value == incomingDeclarationUploadId.Value &&
                    c.IncomingDeclarationState == IncomingDeclarationState.Temporary)
                .ToListAsync(cancellationToken);
        }

        public Task DeleteTemporaryIncomingDeclarationsAsync(CancellationToken cancellationToken = default)
        {
            documentSession.DeleteWhere<IncomingDeclaration>(c => c.IncomingDeclarationState == IncomingDeclarationState.Temporary);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<IncomingDeclaration>> GetAllByProductCountryDispatchLocationDispatchDateAndCapacityAsync(Product product, Country country, PlaceOfDispatch placeOfDispatch, DateOnly startDate, DateOnly endDate, bool isOrderByDescending, OrderByProperty orderByProperty,  CancellationToken cancellationToken = default)
        {
            var result = Query().Where(c => c.Product.Value == product.Value && c.Country.Value == country.Value && (placeOfDispatch.Value == "EXTERNAL" || c.PlaceOfDispatch.Value == placeOfDispatch.Value) && c.DateOfDispatch.Value >= startDate && c.DateOfDispatch.Value <= endDate && c.RemainingVolume > 0 && c.IncomingDeclarationState >= IncomingDeclarationState.Reconciled);

            if (isOrderByDescending)
            {
                result = result.OrderByDescending(orderByProperty.Value.FirstCharToUpper());
            }
            else
            {
                result = result.OrderBy(orderByProperty.Value.FirstCharToUpper());
            }

            return await result.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetAllByDateRangeAndFilterAsync(DateOnly startDate, DateOnly endDate, string product, string company, CancellationToken cancellationToken = default)
        {
            var query = Query().Where(c => c.DateOfDispatch.Value >= startDate && c.DateOfDispatch.Value <= endDate && c.RemainingVolume > 0 && c.IncomingDeclarationState >= IncomingDeclarationState.Reconciled);
            
            if (!string.IsNullOrEmpty(product))
            {
                query = query.Where(d => d.Product.Value.ToUpperInvariant().Contains(product.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }
    
            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(d => d.Company.Value.ToUpperInvariant().Contains(company.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await Query().Where(c => c.Id.In(ids.ToArray())).ToListAsync(cancellationToken);
        }
    }
}