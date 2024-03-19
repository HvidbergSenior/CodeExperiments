
namespace Insight.BuildingBlocks.Integration.Inbox
{
    public class FakeInbox : IInbox
    {
        public IDictionary<Guid, InboxMessage> Messages { get; }

        public FakeInbox()
        {
            Messages = new Dictionary<Guid, InboxMessage>();
        }

        public void Add(InboxMessage inboxMessage)
        {
            if (inboxMessage is null)
            {
                throw new ArgumentNullException(nameof(inboxMessage));
            }

            Messages.Add(inboxMessage.Id, inboxMessage);
        }

        public IEnumerable<InboxMessage> GetUnProcessedMessages()
        {
            return Messages.Values.Where(x => x.ProcessedDate == null).ToList();
        }

        public void Processed(IEnumerable<InboxMessage> messages)
        {
            if (messages is null)
            {
                throw new ArgumentNullException(nameof(messages));
            }
            foreach (var item in messages)
            {
                Messages.Add(item.Id, item);
            }
        }

        public void Processed(InboxMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);
            Messages.Add(message.Id, message);
        }

        public Task CleanUpOldMessagesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
