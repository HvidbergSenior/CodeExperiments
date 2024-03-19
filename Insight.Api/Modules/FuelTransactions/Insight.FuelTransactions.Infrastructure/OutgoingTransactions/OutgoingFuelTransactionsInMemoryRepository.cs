using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;

namespace Insight.FuelTransactions.Infrastructure.OutgoingTransactions
{
    public class OutgoingFuelTransactionsInMemoryRepository : IOutgoingFuelTransactionsRepository
    {
        private readonly List<OutgoingFuelTransaction> outgoingFuelTransactions;

        public OutgoingFuelTransactionsInMemoryRepository(List<OutgoingFuelTransaction> outgoingFuelTransactions)
        {
            this.outgoingFuelTransactions = outgoingFuelTransactions;
        }

        public Task<(IReadOnlyList<OutgoingFuelTransaction> Items, int TotalCount, decimal TotalQuantity)> GetByAggregatedFuelTransactionsAsync(int page, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken = default)
        {
            // Todo: Implement.
            return Task.FromResult<(IReadOnlyList<OutgoingFuelTransaction> Items, int TotalCount, decimal TotalQuantity)>((outgoingFuelTransactions.ToList(), outgoingFuelTransactions.Count, outgoingFuelTransactions.Sum(c=> c.Quantity.Value)));
        }
      
    }
}
