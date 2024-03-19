namespace Insight.BuildingBlocks.Integration.Inbox
{
    public interface IInbox
    {
        void Add(InboxMessage inboxMessage);

        IEnumerable<InboxMessage> GetUnProcessedMessages();

        void Processed(IEnumerable<InboxMessage> messages);
        void Processed(InboxMessage message);

        Task CleanUpOldMessagesAsync();
    }
}
