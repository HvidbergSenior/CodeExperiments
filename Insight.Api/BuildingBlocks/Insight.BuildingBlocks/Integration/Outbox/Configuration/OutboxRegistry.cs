using Marten;

namespace Insight.BuildingBlocks.Integration.Outbox.Configuration
{
    internal sealed class OutboxRegistry : MartenRegistry
    {
        public OutboxRegistry()
        {
            const string schemaname = "integration";

            For<OutboxMessage>().DatabaseSchemaName(schemaname);
            For<OutboxMessage>().UseOptimisticConcurrency(enabled: true);
            For<OutboxMessage>().Duplicate(x => x.OccurredOn);
            For<OutboxMessage>().Duplicate(x => x.ProcessedDate!);
        }
    }
}
