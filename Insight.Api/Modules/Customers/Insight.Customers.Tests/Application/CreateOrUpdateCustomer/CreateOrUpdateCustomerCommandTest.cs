using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.Customers.Application.CreateOrUpdateCustomer;
using Insight.Customers.Domain;
using Insight.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using CustomerId = Insight.BuildingBlocks.Domain.CustomerId;

namespace Insight.Customers.Tests.Application.CreateOrUpdateCustomer
{
    public class CreateOrUpdateCustomerCommandTest
    {
        [Fact]
        public async Task Customer_should_be_created()
        {
            //Arrange
            var unitOfWork = new FakeUnitOfWork();
            var customerRepository = new CustomerInMemoryRepository();
            var logger = NullLogger<CreateOrUpdateCustomerCommandHandler>.Instance;
            var customerHierarchy = new Mock<ICustomerHierarchy>();
            var handler = new CreateOrUpdateCustomerCommandHandler(unitOfWork, customerRepository, logger, customerHierarchy.Object);
            var createCreateOrUpdateCommand =
                CreateCreateOrUpdateCommand(Guid.NewGuid(), CustomerNumber.Create("CustomerNumber"));
            customerHierarchy.Setup(x => x.ClearCache());
            //Act
            await handler.Handle(createCreateOrUpdateCommand, CancellationToken.None);

            //Assert
            customerRepository.Entities.Count.Should().Be(1);
            customerRepository.SaveChangesCalled.Should().BeTrue();
            customerHierarchy.Verify(x => x.ClearCache(), Times.Once);
        }

        [Fact]
        public async Task Customer_should_be_updated()
        {
            //Arrange
            var unitOfWork = new FakeUnitOfWork();
            var customerRepository = new CustomerInMemoryRepository();
            var logger = NullLogger<CreateOrUpdateCustomerCommandHandler>.Instance;
            var customerHierarchy = new Mock<ICustomerHierarchy>();
            customerHierarchy.Setup(x => x.ClearCache());
            var handler = new CreateOrUpdateCustomerCommandHandler(unitOfWork, customerRepository, logger, customerHierarchy.Object);
            var customer = Any.Customer();
            var createCreateOrUpdateCommand =
                CreateCreateOrUpdateCommand(customer.CompanyId.Value, customer.CustomerDetails.CustomerNumber);
            await customerRepository.Add(customer);

            //Act
            await handler.Handle(createCreateOrUpdateCommand, CancellationToken.None);

            //Assert
            customerRepository.Entities.Count.Should().Be(1);
            customerRepository.SaveChangesCalled.Should().BeTrue();
            customerHierarchy.Verify(x => x.ClearCache(), Times.Once);
        }

        private static CreateOrUpdateCustomerCommand CreateCreateOrUpdateCommand(Guid companyId,
            CustomerNumber customerNumber)
        {
            return CreateOrUpdateCustomerCommand.Create(
                CompanyId.Create(companyId),
                CustomerId.Create(Guid.NewGuid()),
                Blocked.Create("Blocked"),
                PaymentTermsCode.Create("CreatedOrUpdatedPayment"),
                ShipmentMethodCode.Create("shipment"),
                ShippingAgentCode.Create("Shipping"),
                CreditLimit.Create(3333),
                CustomerDetails.Create(
                    customerNumber,
                    CustomerAddress.Create("CAddr"),
                    CustomerBillToName.Create("BillToName"),
                    CustomerBillToNumber.Create("BillToNumber"),
                    CustomerCity.Create("City"),
                    CustomerDeliveryType.Create("DeliveryType"),
                    CustomerIndustry.Create("Industry"),
                    CustomerName.Create("Name"),
                    CustomerPostCode.Create("PostCode"),
                    CustomerCountryRegion.Create("Region")
                ),
                SourceETag.Create("Etag"),
                BalanceLcy.Create(33333),
                BalanceDueLcy.Create(6666),
                OutstandingOrdersLcy.Create(45645654),
                VatRegNumber.Create("VatRegNumber"),
                NumberNumber.Create("VatReg"),
                OrganisationNumber.Create("OrganiNumber"),
                PdiAndLdPointNumber.Create("Pdi"),
                SalesPerson.Create("SalesPerson"),
                CardCustomer.Create(true));
        }
    }
}