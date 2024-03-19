using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.Customers.Domain;
using Insight.Services.BusinessCentralConnector.Integration;

namespace Insight.Customers.Application.AllowedRawMaterials
{
    public class AllowedRawMaterialUpdated : IIntegrationEventListener<AllowedRawMaterialUpdatedIntegrationEvent>
    {
        private readonly IInbox inbox;

        public AllowedRawMaterialUpdated(IInbox inbox)
        {
            this.inbox = inbox;
        }

        public Task Handle(AllowedRawMaterialUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var cmd = CreateCommand(notification);
            inbox.Add(InboxMessage.From(cmd, notification.Id));
            return Task.CompletedTask;
        }

        private static AllowedRawMaterialUpdatedCommand CreateCommand(AllowedRawMaterialUpdatedIntegrationEvent notification)
        {
            var companyId = CompanyId.Create(notification.CompanyId);
            var customerNumber = CustomerNumber.Create(notification.CustomerNumber);
            var allowedRawMaterials = Domain.AllowedRawMaterials.Create(notification.AllowedRawMaterials);

            return AllowedRawMaterialUpdatedCommand.Create(companyId, customerNumber, allowedRawMaterials);
        }
    }
}
