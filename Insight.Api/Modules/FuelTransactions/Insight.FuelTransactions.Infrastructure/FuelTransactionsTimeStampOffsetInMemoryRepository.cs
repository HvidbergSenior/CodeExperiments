using Insight.BuildingBlocks.Fakes;
using Insight.FuelTransactions.Domain;

namespace Insight.FuelTransactions.Infrastructure
{
    public class FuelTransactionsTimeStampOffsetInMemoryRepository : InMemoryRepository<FuelTransactionsOffset>, IFuelTransactionsTimeStampOffsetRepository
    {
        public Task<FuelTransactionsOffset?> GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem fuelTransactionPosSystem, CancellationToken cancellationToken)
        {
            return Task.FromResult(Query().FirstOrDefault(c => c.FuelTransactionPosSystem == fuelTransactionPosSystem));
        }
    }
}
