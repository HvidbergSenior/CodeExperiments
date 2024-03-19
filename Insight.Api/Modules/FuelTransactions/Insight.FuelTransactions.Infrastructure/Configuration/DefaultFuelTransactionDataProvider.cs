using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.FuelTransactions.Domain;
using Marten;

namespace Insight.FuelTransactions.Infrastructure.Configuration
{
    public sealed class DefaultFuelTransactionDataProvider : IDefaultDataProvider
    {
        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using (var ds = documentStore.LightweightSession())
            {
                // Init the table.
                await ds.Query<FuelTransaction>().FirstOrDefaultAsync(cancellation);
            }
        }
    }
}
