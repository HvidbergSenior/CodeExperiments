using System.Globalization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.OutgoingDeclarations.Domain;
using Marten;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.OutgoingDeclarations.Infrastructure
{
    public class OutgoingDeclarationInMemoryRepository : InMemoryRepository<OutgoingDeclaration>, IOutgoingDeclarationRepository
    {
        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }
        public async Task<IEnumerable<OutgoingDeclaration>> GetByCustomerNumberAsync(string customerNumberValue, CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query().Where(c => c.CustomerDetails.CustomerNumber.Value == customerNumberValue), cancellationToken);
        }
        public async Task<IEnumerable<OutgoingDeclaration>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query(), cancellationToken);
        }
        
        public async Task<(IEnumerable<OutgoingDeclaration> Items, int TotalCount, bool HasMore)> GetOutgoingDeclarationsByPageNumberAndPageSize(int pageNumber, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var query = Query();

            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                query = query.Where(declaration => declaration.DateOfIssuance.Value >= filteringParameters.DatePeriod.StartDate && declaration.DateOfIssuance.Value <= filteringParameters.DatePeriod.EndDate);
            }

            if (filteringParameters.Product.Value != Product.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                query = query.Where(d => d.Product.Value.Contains(filteringParameters.Product.Value));
            }

            if (filteringParameters.CustomerName.Value.ToLower(CultureInfo.InvariantCulture) != CustomerName.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                query = query.Where(d => d.CustomerDetails.CustomerName.Value.Contains(filteringParameters.CustomerName.Value));
            } 
           
            //TODO: Uncommented in InMemoryRep because of error: "Failed to compare two elements in the array -> check if it works in Marten"
            // query = sortingParameters.IsOrderDescending
            //     ? query.OrderByDescending(p => sortingParameters.OrderByProperty.FirstCharToUpper())
            //     : query.OrderBy(p => sortingParameters.OrderByProperty.FirstCharToUpper());

            var items = await IQueryableExtension.ToMartenListAsync(query, cancellationToken);
            
            return (items, Convert.ToInt32(items.Count), pageSize - items.Count <= 0 );
        }

        public Task<IEnumerable<OutgoingDeclaration>> GetOutgoingDeclarationsForMany(FilteringParametersSelectMany filteringParameters,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}