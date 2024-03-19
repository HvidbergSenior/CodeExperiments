using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.Services.BusinessCentralConnector.Integration;

namespace Insight.UserAccess.Application.CreateOrUpdateCustomer;

public class CustomerCreatedOrUpdated : IIntegrationEventListener<CustomerCreatedOrUpdatedIntegrationEvent>
{
    private readonly IInbox inbox;

    public CustomerCreatedOrUpdated(IInbox inbox)
    {
        this.inbox = inbox;
    }

    public Task Handle(CustomerCreatedOrUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var cmd = CreateCommand(notification);
        inbox.Add(InboxMessage.From(cmd, notification.Id));
        return Task.CompletedTask;
    }

    private static CreateOrUpdateCustomerCommand CreateCommand(CustomerCreatedOrUpdatedIntegrationEvent notification)
    {
        return CreateOrUpdateCustomerCommand.Create(CustomerId.Create(notification.Id),
            CustomerNumber.Create(notification.CustomerNumber),
            CustomerName.Create(notification.CustomerName));
    }
}
