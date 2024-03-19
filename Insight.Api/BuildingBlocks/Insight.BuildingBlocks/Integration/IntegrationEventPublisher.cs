using MediatR;

namespace Insight.BuildingBlocks.Integration
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IPublisher _publisher;

        public IntegrationEventPublisher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            return _publisher.Publish(integrationEvent, cancellationToken);
        }
    }
}
