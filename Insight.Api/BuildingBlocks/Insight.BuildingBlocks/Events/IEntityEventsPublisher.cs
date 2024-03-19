using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Events
{
    public interface IEntityEventsPublisher
    {
        IReadOnlyCollection<IDomainEvent> EnqueueEventsFrom(Entity entity);

        bool TryEnqueueEventsFrom(Entity entity, out IReadOnlyCollection<IDomainEvent> uncomittedEvents);

        Task Publish(CancellationToken cancellationToken = default);
    }
}