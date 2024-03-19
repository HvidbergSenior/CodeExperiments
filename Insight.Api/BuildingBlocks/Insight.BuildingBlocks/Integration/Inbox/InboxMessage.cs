using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.Generators;

namespace Insight.BuildingBlocks.Integration.Inbox
{
    public sealed class InboxMessage
    {
        public Guid Id { get; private set; }

        public Guid IntegrationEventId { get; private set; }

        public DateTimeOffset OccurredOn { get; private set; }
        public string MessageType { get; private set; } = default!;

        public string Payload { get; private set; } = default!;

        public DateTimeOffset? ProcessedDate { get; private set; }

        public int RetryCount { get; private set; }

        private InboxMessage()
        { }

        private InboxMessage(Guid id, DateTimeOffset occurredOn, string messageType, string payload, Guid integrationEventId)
        {
            Id = id;
            OccurredOn = occurredOn;
            MessageType = messageType;
            Payload = payload;
            IntegrationEventId = integrationEventId;
        }

        internal void Processed()
        {
            ProcessedDate = DateTimeOffset.UtcNow;
        }

        public void IncreaseRetryCount()
        {
            RetryCount++;
        }

        public static InboxMessage From(IInternalCommand cmd, Guid integrationEventId)
        {
            if (cmd is null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            var messageType = cmd.GetType().AssemblyQualifiedName;
            var type = Type.GetType(messageType!);

            var serializer = new InboxMessageSerializer();
            var payload = serializer.Serialize(cmd, type);
            var id = new SequentialGuidIdGenerator().Generate();
            return new InboxMessage(id, DateTimeOffset.UtcNow, messageType!, payload, integrationEventId);
        }
    }
}
