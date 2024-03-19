using Marten;

namespace Insight.BuildingBlocks.Integration.Inbox.Configuration
{
    internal sealed class InboxRegistry : MartenRegistry
    {
        public InboxRegistry()
        {
            const string schemaname = "integration";

            For<InboxMessage>().DatabaseSchemaName(schemaname);
            For<InboxMessage>().UseOptimisticConcurrency(enabled: true);
            For<InboxMessage>().Duplicate(x => x.OccurredOn);
            For<InboxMessage>().Duplicate(x => x.ProcessedDate!);
            For<InboxMessage>().UniqueIndex(x => x.IntegrationEventId, x => x.MessageType);
        }
    }
}
