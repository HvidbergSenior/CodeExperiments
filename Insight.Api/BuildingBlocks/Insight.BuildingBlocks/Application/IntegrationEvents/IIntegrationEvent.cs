using MediatR;

namespace Insight.BuildingBlocks.Application.IntegrationEvents
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }

        DateTimeOffset OccurredOn { get; }
    }
}
