using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders
{
    public class CustomerDefaultDataProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<CustomerDefaultDataProvider> logger;

        public CustomerDefaultDataProvider(IServiceScopeFactory serviceScopeFactory, ILogger<CustomerDefaultDataProvider> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var businessCentralCustomerService = scope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();

                var customers = await businessCentralCustomerService.GetAllAsync(cancellation);

                foreach (var customer in customers)
                {
                    var customerFoundIntegrationEvent = CreateCustomerIntegrationEvent(customer);
                    var outboxMessage = OutboxMessage.From(customerFoundIntegrationEvent);
                    outbox.Add(outboxMessage);
                }

                await unitOfWork.Commit(cancellation);
                logger.LogInformation("Added {Count} customers to outbox", customers.Count());
            }
        }

        private static CustomerCreatedOrUpdatedIntegrationEvent CreateCustomerIntegrationEvent(
        BusinessCentralCustomer customer)
        {
            return CustomerCreatedOrUpdatedIntegrationEvent.Create(
                customer.CompanyId,
                customer.Etag,
                customer.Number,
                customer.CustomerNumber,
                customer.CustomerName,
                customer.CustomerAddress,
                customer.CustomerCity,
                customer.CustomerPostCode,
                customer.CustomerCountryRegion,
                customer.CustomerBillToNumber,
                customer.CustomerDeliveryType,
                customer.CustomerIndustry,
                customer.CustomerBillToName,
                customer.PaymentTermsCode,
                customer.CreditLimit,
                customer.CardCustomer,
                customer.ShipmentMethodCode,
                customer.ShippingAgentCode,
                customer.OrganisationNumber,
                customer.BalanceLCY,
                customer.BalanceDueLCY,
                customer.Blocked,
                customer.OutstandingOrdersLCY,
                customer.SalesPerson,
                customer.PDIAndLDPointNumber,
                customer.VATRegNumber);
        }
    }
}