using Marten;

namespace Insight.BuildingBlocks.Integration.Outbox.Marten
{
    internal sealed class MartenOutbox : IOutbox
    {
        private readonly IDocumentSession _session;

        public MartenOutbox(IDocumentSession session)
        {
            _session = session;
        }

        public async Task CleanUpOldMessagesAsync()
        {
            _session.DeleteWhere<OutboxMessage>(c => c.ProcessedDate != null && c.ProcessedDate < DateTimeOffset.UtcNow.AddDays(-7));
            await _session.SaveChangesAsync();
        }

        public void Add(OutboxMessage message)
        {
            _session.Insert(message);
        }

        public IEnumerable<OutboxMessage> GetUnProcessedMessages()
        {
            return _session.Query<OutboxMessage>().Where(x => x.ProcessedDate == null).ToList();
        }

        public void Processed(IEnumerable<OutboxMessage> messages)
        {
            _session.Update(messages);
        }

        public Task Save()
        {
            return Task.CompletedTask;
        }
    }
}
