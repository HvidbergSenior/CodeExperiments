using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Outbox;

namespace Insight.BuildingBlocks.Tests.Integration
{
    public class OutboxMessageTests
    {
        private sealed class TestIntegrationEvent : IntegrationEvent
        {
            public TestIntegrationEvent(Guid id, DateTimeOffset occurredOn) : base(id, occurredOn)
            {
            }
        }

        [Fact]
        public void Will_Create_OutboxMessage_From_IntegrationEvent_With_Same_Id()
        {
            var @event = new TestIntegrationEvent(Guid.NewGuid(), DateTimeOffset.Now);
            var message = OutboxMessage.From(@event);
            Assert.NotNull(message);
            Assert.Equal(@event.Id, message.Id);
        }

        [Fact]
        public void Will_serialize_and_deserialize()
        {
            var @event = new TestIntegrationEvent(Guid.NewGuid(), DateTimeOffset.Now);
            var message = OutboxMessage.From(@event);
            var serializer = new OutboxMessageSerializer();

            var eventFromJson = serializer.Deserialize(message.Payload, message.MessageType)!;
            var fromJson = eventFromJson as IntegrationEvent;

            Assert.NotNull(eventFromJson);
            Assert.Equal(@event.OccurredOn, fromJson!.OccurredOn);
            Assert.Equal(@event.Id, fromJson!.Id);
        }
    }
}