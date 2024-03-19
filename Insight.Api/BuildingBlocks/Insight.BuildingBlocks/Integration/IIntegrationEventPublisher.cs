namespace Insight.BuildingBlocks.Integration
{
    public interface IIntegrationEventPublisher
    {
        Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}
