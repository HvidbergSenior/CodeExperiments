using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.Customers.Domain;
using Insight.Services.BusinessCentralConnector.Integration;
using CustomerName = Insight.BuildingBlocks.Domain.CustomerName;

namespace Insight.Customers.Application.CreateOrUpdateCustomer;

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
        return CreateOrUpdateCustomerCommand.Create(
            CompanyId.Create(notification.CompanyId),
            CustomerId.Create(notification.Id),
            Blocked.Create(notification.Blocked),
            PaymentTermsCode.Create(notification.PaymentTermsCode),
            ShipmentMethodCode.Create(notification.ShipmentMethodCode),
            ShippingAgentCode.Create(notification.ShippingAgentCode),
            CreditLimit.Create(notification.CreditLimit),
            CustomerDetails.Create(
                CustomerNumber.Create(notification.CustomerNumber),
                CustomerAddress.Create(notification.CustomerAddress),
                CustomerBillToName.Create(notification.CustomerBillToName),
                CustomerBillToNumber.Create(notification.CustomerBillToNumber),
                CustomerCity.Create(notification.CustomerCity),
                CustomerDeliveryType.Create(notification.CustomerDeliveryType),
                CustomerIndustry.Create(notification.CustomerIndustry),
                CustomerName.Create(notification.CustomerName),
                CustomerPostCode.Create(notification.CustomerPostCode),
                CustomerCountryRegion.Create(notification.CustomerCountryRegion)
            ),
            SourceETag.Create(notification.SourceETag),
            BalanceLcy.Create(notification.BalanceLcy),
            BalanceDueLcy.Create(notification.BalanceDueLcy),
            OutstandingOrdersLcy.Create(notification.OutstandingOrdersLcy),
            VatRegNumber.Create(notification.VatRegNumber),
            NumberNumber.Create(notification.Number),
            OrganisationNumber.Create(notification.OrganisationNumber),
            PdiAndLdPointNumber.Create(notification.PdiAndLdPointNumber),
            SalesPerson.Create(notification.SalesPerson),
            CardCustomer.Create(notification.CardCustomer)
        );
    }
}
