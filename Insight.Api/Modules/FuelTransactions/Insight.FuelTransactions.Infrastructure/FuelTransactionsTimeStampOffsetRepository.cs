using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.FuelTransactions.Domain;
using Marten;

namespace Insight.FuelTransactions.Infrastructure
{
    public class FuelTransactionsTimeStampOffsetRepository : MartenDocumentRepository<FuelTransactionsOffset>,
        IFuelTransactionsTimeStampOffsetRepository
    {
        public FuelTransactionsTimeStampOffsetRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public async Task<FuelTransactionsOffset?> GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem fuelTransactionPosSystem, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(c => c.FuelTransactionPosSystem == fuelTransactionPosSystem, cancellationToken);
        }
    }
}