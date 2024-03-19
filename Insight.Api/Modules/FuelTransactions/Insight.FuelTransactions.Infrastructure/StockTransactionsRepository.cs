using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Stock;
using Marten;
using Marten.Pagination;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Infrastructure
{
    public class StockTransactionsRepository : MartenDocumentRepository<StockTransaction>, IStockTransactionsRepository
    {
        private readonly IDocumentSession documentSession;

        public StockTransactionsRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task<(IEnumerable<StockTransaction> Items, int TotalCount)> GetStockTransactionsAsync(int page, int pageSize, SortingParameters sortingParameters, StockFilteringParameters filteringParameters, CancellationToken cancellationToken = default)
        {
            page = page == 0 ? 1 : page;

            var query = Query();

            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                query = query.Where(stock =>
                    stock.TransactionDate.Value >= filteringParameters.DatePeriod.StartDate &&
                    stock.TransactionDate.Value <= filteringParameters.DatePeriod.EndDate);
            }

            if (!filteringParameters.ProductName.Value.ToUpperInvariant().Equals(ProductName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.ProductName.Value.ToUpperInvariant().Contains(filteringParameters.ProductName.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            if (!filteringParameters.CompanyName.Value.ToUpperInvariant().Equals(CompanyName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.CompanyName.Value.ToUpperInvariant().Contains(filteringParameters.CompanyName.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            query = sortingParameters.IsOrderDescending
                ? query.OrderByDescending(sortingParameters.OrderByProperty.FirstCharToUpper())
                : query.OrderBy(sortingParameters.OrderByProperty.FirstCharToUpper());

            var pagedList = await query.ToPagedListAsync(page, pageSize, cancellationToken);

            return (pagedList, (int)pagedList.TotalItemCount);
        }
    }
}
