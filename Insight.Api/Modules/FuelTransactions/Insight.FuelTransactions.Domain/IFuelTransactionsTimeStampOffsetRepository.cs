using Insight.BuildingBlocks.Infrastructure;

namespace Insight.FuelTransactions.Domain;

public interface IFuelTransactionsTimeStampOffsetRepository: IRepository<FuelTransactionsOffset>, IReadonlyRepository<FuelTransactionsOffset>
{
    Task<FuelTransactionsOffset?> GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem fuelTransactionPosSystem, CancellationToken cancellationToken);
}