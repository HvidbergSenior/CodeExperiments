using MediatR;

namespace Insight.BuildingBlocks.Domain
{
    public interface IDomainEventListener<in TRequest> : INotificationHandler<TRequest> where TRequest : IDomainEvent
    {
    }
}
