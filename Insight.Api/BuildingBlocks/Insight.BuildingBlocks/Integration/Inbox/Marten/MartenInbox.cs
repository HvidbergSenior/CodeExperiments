using Marten;

namespace Insight.BuildingBlocks.Integration.Inbox.Marten
{
    internal sealed class MartenInbox : IInbox
    {
        private readonly IDocumentSession _documentSession;

        public MartenInbox(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Add(InboxMessage inboxMessage)
        {
            _documentSession.Insert(inboxMessage);
        }

        public async Task CleanUpOldMessagesAsync()
        {
            _documentSession.DeleteWhere<InboxMessage>(c => c.ProcessedDate != null && c.ProcessedDate < DateTimeOffset.UtcNow.AddDays(-7));
            await _documentSession.SaveChangesAsync();
        }

        public IEnumerable<InboxMessage> GetUnProcessedMessages()
        {
            return _documentSession.Query<InboxMessage>()
                .Where(x => x.ProcessedDate == null)
                .OrderBy(x => x.OccurredOn)
                .ToList();
        }

        public void Processed(IEnumerable<InboxMessage> messages)
        {
            _documentSession.Update(messages);
        }
        public void Processed(InboxMessage message)
        {
            _documentSession.Update(message);
        }
    }
}
