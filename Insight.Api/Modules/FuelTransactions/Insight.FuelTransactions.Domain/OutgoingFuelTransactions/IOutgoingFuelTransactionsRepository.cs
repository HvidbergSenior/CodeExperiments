namespace Insight.FuelTransactions.Domain.OutgoingFuelTransactions
{
    public interface IOutgoingFuelTransactionsRepository
    {
        public Task<(IReadOnlyList<OutgoingFuelTransaction> Items, int TotalCount, decimal TotalQuantity)> GetByAggregatedFuelTransactionsAsync(int page, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters,
        CancellationToken cancellationToken = default);
    }
}
