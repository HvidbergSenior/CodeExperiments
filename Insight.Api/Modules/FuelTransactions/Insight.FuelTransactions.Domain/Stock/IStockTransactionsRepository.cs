using Insight.BuildingBlocks.Infrastructure;

namespace Insight.FuelTransactions.Domain.Stock
{
    public interface IStockTransactionsRepository : IRepository<StockTransaction>, IReadonlyRepository<StockTransaction>
    {
        public Task<(IEnumerable<StockTransaction> Items, int TotalCount)> GetStockTransactionsAsync(int page, int pageSize, SortingParameters sortingParameters, StockFilteringParameters filteringParameters, CancellationToken cancellationToken = default);
    }
}
