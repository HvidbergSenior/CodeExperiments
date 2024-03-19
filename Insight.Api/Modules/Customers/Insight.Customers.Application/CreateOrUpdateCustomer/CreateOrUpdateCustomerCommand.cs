using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Insight.Customers.Application.CreateOrUpdateCustomer
{
    public sealed class CreateOrUpdateCustomerCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        internal CompanyId CompanyId { get; set; }
        internal CustomerId CustomerId { get; set; }
        internal Blocked Blocked { get; set; }
        internal PaymentTermsCode PaymentTermsCode { get; set; }
        internal ShipmentMethodCode ShipmentMethodCode { get; set; }
        internal ShippingAgentCode ShippingAgentCode { get; set; }
        internal CreditLimit CreditLimit { get; set; }
        internal CustomerDetails CustomerDetails { get; set; }
        internal SourceETag SourceETag { get; set; }
        internal BalanceLcy BalanceLcy { get; set; }
        internal BalanceDueLcy BalanceDueLcy { get; set; }
        internal OutstandingOrdersLcy OutstandingOrdersLcy { get; set; }
        internal VatRegNumber VatRegNumber { get; set; }
        internal NumberNumber NumberNumber { get; set; }
        internal OrganisationNumber OrganisationNumber { get; set; }
        internal PdiAndLdPointNumber PdiAndLdPointNumber { get; set; }
        internal SalesPerson SalesPerson { get; set; }
        internal CardCustomer CardCustomer { get; set; }

#pragma warning disable CS8618
        private CreateOrUpdateCustomerCommand()
#pragma warning restore CS8618
        {
            //Left empty for serialization purposes
        }

        private CreateOrUpdateCustomerCommand(
            CompanyId companyId,
            CustomerId customerId,
            Blocked blocked,
            PaymentTermsCode paymentTermsCode,
            ShipmentMethodCode shipmentMethodCode,
            ShippingAgentCode shippingAgentCode,
            CreditLimit creditLimit,
            CustomerDetails customerDetails,
            SourceETag sourceETag,
            BalanceLcy balanceLcy,
            BalanceDueLcy balanceDueLcy,
            OutstandingOrdersLcy outstandingOrdersLcy,
            VatRegNumber vatRegNumber,
            NumberNumber numberNumber,
            OrganisationNumber organisationNumber,
            PdiAndLdPointNumber pdiAndLdPointNumber,
            SalesPerson salesPerson,
            CardCustomer cardCustomer)
        {
            CompanyId = companyId;
            CustomerId = customerId;
            Blocked = blocked;
            PaymentTermsCode = paymentTermsCode;
            ShipmentMethodCode = shipmentMethodCode;
            ShippingAgentCode = shippingAgentCode;
            CreditLimit = creditLimit;
            CustomerDetails = customerDetails;
            SourceETag = sourceETag;
            BalanceLcy = balanceLcy;
            BalanceDueLcy = balanceDueLcy;
            OutstandingOrdersLcy = outstandingOrdersLcy;
            VatRegNumber = vatRegNumber;
            NumberNumber = numberNumber;
            OrganisationNumber = organisationNumber;
            PdiAndLdPointNumber = pdiAndLdPointNumber;
            SalesPerson = salesPerson;
            CardCustomer = cardCustomer;
        }

        public static CreateOrUpdateCustomerCommand Create(
            CompanyId companyId,
            CustomerId customerId,
            Blocked blocked,
            PaymentTermsCode paymentTermsCode,
            ShipmentMethodCode shipmentMethodCode,
            ShippingAgentCode shippingAgentCode,
            CreditLimit creditLimit,
            CustomerDetails customerDetails,
            SourceETag sourceETag,
            BalanceLcy balanceLcy,
            BalanceDueLcy balanceDueLcy,
            OutstandingOrdersLcy outstandingOrdersLcy,
            VatRegNumber vatRegNumber,
            NumberNumber numberNumber,
            OrganisationNumber organisationNumber,
            PdiAndLdPointNumber pdiAndLdPointNumber,
            SalesPerson salesPerson,
            CardCustomer cardCustomer
        )
        {
            return new CreateOrUpdateCustomerCommand(
                companyId,
                customerId,
                blocked,
                paymentTermsCode,
                shipmentMethodCode,
                shippingAgentCode,
                creditLimit,
                customerDetails,
                sourceETag,
                balanceLcy,
                balanceDueLcy,
                outstandingOrdersLcy,
                vatRegNumber,
                numberNumber,
                organisationNumber,
                pdiAndLdPointNumber,
                salesPerson,
                cardCustomer);
        }
    }

    internal class CreateOrUpdateCustomerCommandHandler : ICommandHandler<CreateOrUpdateCustomerCommand, ICommandResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<CreateOrUpdateCustomerCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICustomerHierarchy customerHierarchy;

        public CreateOrUpdateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository, ILogger<CreateOrUpdateCustomerCommandHandler> logger, ICustomerHierarchy customerHierarchy)
        {
            this.unitOfWork = unitOfWork;
            this.customerRepository = customerRepository;
            this.logger = logger;
            this.customerHierarchy = customerHierarchy;
        }

        public async Task<ICommandResponse> Handle(CreateOrUpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = CreateNewCustomer(request);

            var customerNumber = CustomerNumber.Create(customer.CustomerDetails.CustomerNumber.Value);

            var existingCustomer =
                await customerRepository.GetByCustomerNumberAndCompanyIdAsync(customerNumber, customer.CompanyId,
                    cancellationToken);

            if (existingCustomer != null)
            {
                existingCustomer.UpdateCustomer(
                    existingCustomer.CompanyId, // We don't want to update the company id
                    existingCustomer.CustomerId, // We don't want to update the customer id
                    customer.CustomerDetails,
                    customer.BalanceLcy,
                    customer.BalanceDueLcy,
                    customer.OutstandingOrdersLcy,
                    customer.NumberNumber,
                    customer.PdiAndLdPointNumber,
                    customer.VatRegNumber,
                    customer.OrganisationNumber,
                    customer.PaymentTermsCode,
                    customer.ShipmentMethodCode,
                    customer.ShippingAgentCode,
                    customer.SalesPerson,
                    customer.SourceETag,
                    customer.CreditLimit,
                    customer.Blocked,
                    customer.CardCustomer);

                await customerRepository.Update(existingCustomer, cancellationToken);
                logger.LogInformation("Customer {CustomerName} ({CustomerNumber}) in company {CompanyId} updated",
                                       existingCustomer.CustomerDetails.CustomerName.Value, existingCustomer.CustomerDetails.CustomerNumber.Value, existingCustomer.CompanyId.Value);
            }
            else
            {
                logger.LogInformation("Customer {CustomerNumber} created", customer.NumberNumber.Value);
                await customerRepository.Add(customer, cancellationToken);
                logger.LogInformation("Customer {CustomerName} ({CustomerNumber}) in company {CompanyId} created",
                                                          customer.CustomerDetails.CustomerName.Value, customer.CustomerDetails.CustomerNumber.Value, customer.CompanyId.Value);
            }

            await customerRepository.SaveChanges(cancellationToken);
            customerHierarchy.ClearCache();
            
            return EmptyCommandResponse.Default;
        }

        private static Customer CreateNewCustomer(CreateOrUpdateCustomerCommand request)
        {
            return Customer.Create(
                request.CompanyId,
                request.CustomerId,
                request.CustomerDetails,
                request.BalanceLcy,
                request.BalanceDueLcy,
                request.OutstandingOrdersLcy,
                request.NumberNumber,
                request.PdiAndLdPointNumber,
                request.VatRegNumber,
                request.OrganisationNumber,
                request.PaymentTermsCode,
                request.ShipmentMethodCode,
                request.ShippingAgentCode,
                request.SalesPerson,
                request.SourceETag,
                request.CreditLimit,
                request.Blocked,
                request.CardCustomer);
        }
    }
    internal class CreateOrUpdateCustomerCommandAuthorizer : IAuthorizer<CreateOrUpdateCustomerCommand>
    {
        private readonly IExecutionContext executionContext;

        public CreateOrUpdateCustomerCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(CreateOrUpdateCustomerCommand query,
            CancellationToken cancellation)
        {
            // Todo: This fails when running from the inbox.
            return Task.FromResult(AuthorizationResult.Succeed());            
        }
    }
}