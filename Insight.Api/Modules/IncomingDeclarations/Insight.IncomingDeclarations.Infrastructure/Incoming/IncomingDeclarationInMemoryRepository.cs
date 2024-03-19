using System.Globalization;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class IncomingDeclarationInMemoryRepository : InMemoryRepository<IncomingDeclaration>, IIncomingDeclarationRepository
    {
        public async Task<IEnumerable<IncomingDeclaration>> GetByIncomingDeclarationIdsAndStateAsync(IncomingDeclarationId[] incomingDeclarationIds, IncomingDeclarationState incomingDeclarationState, CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query().Where(query => incomingDeclarationIds.Select(c => c.Value).Contains(query.Id) && query.IncomingDeclarationState == incomingDeclarationState), cancellationToken);
        }

        public async Task<(IEnumerable<IncomingDeclaration> Items, int TotalCount, bool HasMore)> GetIncomingDeclarationsByPageNumberAndPageSize(int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, IEnumerable<IncomingDeclarationState> incomingDeclarationStates, CancellationToken cancellationToken = default)
        {
            var incomingDeclarations = Query();

            incomingDeclarations = incomingDeclarations.Where(query =>
                query.IncomingDeclarationState != IncomingDeclarationState.Temporary);

            if (incomingDeclarationStates.Any())
            {
                incomingDeclarations = incomingDeclarations.Where(declaration =>
                    incomingDeclarationStates.Contains(declaration.IncomingDeclarationState));
            }

            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            { 
                incomingDeclarations = incomingDeclarations.Where(declaration => declaration.DateOfDispatch.Value >= filteringParameters.DatePeriod.StartDate && declaration.DateOfDispatch.Value <= filteringParameters.DatePeriod.EndDate);
            }

            if (filteringParameters.Product.Value.ToLower(CultureInfo.InvariantCulture) != Product.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                incomingDeclarations = incomingDeclarations.Where(d => d.Product.Value.Contains(filteringParameters.Product.Value));
            }

            if (filteringParameters.Company.Value.ToLower(CultureInfo.InvariantCulture) != Company.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                incomingDeclarations = incomingDeclarations.Where(d => d.Company.Value.Contains(filteringParameters.Company.Value));
            }

            if (filteringParameters.Supplier.Value.ToLower(CultureInfo.InvariantCulture) != Supplier.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                incomingDeclarations = incomingDeclarations.Where(d => d.Supplier.Value.Contains(filteringParameters.Supplier.Value));
            }

            var totalItemCount = incomingDeclarations.Count();

            if (totalItemCount > 0)
            {
                incomingDeclarations = incomingDeclarations
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                incomingDeclarations = sortingParameters.IsOrderDescending
                    ? incomingDeclarations.OrderByDescending(e => sortingParameters.OrderByProperty)
                        .OfType<IncomingDeclaration>()
                    : incomingDeclarations.OrderBy(e => sortingParameters.OrderByProperty)
                        .OfType<IncomingDeclaration>();
            }

            var items = await IQueryableExtension.ToMartenListAsync(incomingDeclarations, cancellationToken);

            return (items, totalItemCount, pageSize - items.Count <= 0);
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetTemporaryIncomingDeclarationsByUploadIdAsync(IncomingDeclarationUploadId incomingDeclarationUploadId, CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query().Where(c => c.IncomingDeclarationUploadId.Value == incomingDeclarationUploadId.Value && c.IncomingDeclarationState == IncomingDeclarationState.Temporary), cancellationToken);
        }
        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }
    
        public async Task<IEnumerable<IncomingDeclaration>> GetAllByProductCountryDispatchLocationDispatchDateAndCapacityAsync(Product product, Country country, PlaceOfDispatch placeOfDispatch, DateOnly startDate, DateOnly endDate, bool isOrderByDescending, OrderByProperty orderByProperty,  CancellationToken cancellationToken = default)
        {
            var incomingDeclarations = Query().Where(c => c.Product.Value == product.Value && c.Country.Value == country.Value && c.PlaceOfDispatch.Value == placeOfDispatch.Value && c.DateOfDispatch.Value >= startDate && c.DateOfDispatch.Value <= endDate && c.RemainingVolume > 0 && c.IncomingDeclarationState >= IncomingDeclarationState.Reconciled);
            return await IQueryableExtension.ToMartenListAsync(incomingDeclarations, cancellationToken);
        }
        public async Task<IEnumerable<IncomingDeclaration>> GetAllByDateRangeAndFilterAsync(DateOnly startDate, DateOnly endDate, string product, string company, CancellationToken cancellationToken = default)
        {
            //Filtering (product, company) is not implemented in InMemory 
            return await IQueryableExtension.ToMartenListAsync(Query().Where(c => c.DateOfDispatch.Value >= startDate && c.DateOfDispatch.Value <= endDate && c.RemainingVolume > 0 && c.IncomingDeclarationState >= IncomingDeclarationState.Reconciled), cancellationToken);
        }
        public async Task DeleteTemporaryIncomingDeclarationsAsync(CancellationToken cancellationToken = default)
        {
            var declarationsToDelete = await IQueryableExtension.ToMartenListAsync(Query().Where(p => p.IncomingDeclarationState == IncomingDeclarationState.Temporary), cancellationToken);
            await Delete(declarationsToDelete);
        }

        public async Task<IEnumerable<IncomingDeclaration>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await IQueryableExtension.ToMartenListAsync(Query().Where(c => c.Id.In(ids.ToArray())), cancellationToken);
        }
    }
}