using MediatR;

namespace Insight.BuildingBlocks.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTimeOffset OccurredOn { get; }
    }
}
