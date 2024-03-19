using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.Customers.Domain;
using Insight.Services.BusinessCentralConnector.Integration;

namespace Insight.Customers.Application.CO2Targets
{
    public class CO2TargetUpdated : IIntegrationEventListener<CO2TargetUpdatedIntegrationEvent>
    {
        private readonly IInbox inbox;

        public CO2TargetUpdated(IInbox inbox)
        {
            this.inbox = inbox;
        }

        public Task Handle(CO2TargetUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var cmd = CreateCommand(notification);
            inbox.Add(InboxMessage.From(cmd, notification.Id));
            return Task.CompletedTask;
        }

        private static CO2TargetUpdatedCommand CreateCommand(CO2TargetUpdatedIntegrationEvent notification)
        {
            var companyId = CompanyId.Create(notification.CompanyId);
            var customerNumber = CustomerNumber.Create(notification.CustomerNumber);
            var co2Target = CO2Target.Create(notification.CO2Target);
            return CO2TargetUpdatedCommand.Create(customerNumber, companyId, co2Target);
        }
    }
}
