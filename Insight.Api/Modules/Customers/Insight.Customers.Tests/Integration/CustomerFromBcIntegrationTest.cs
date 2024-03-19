using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Customers.Application.CreateOrUpdateCustomer;
using Insight.Customers.Tests.Application;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Insight.Customers.Tests.Integration
{
    public class CustomerFromBcIntegrationTest
    {
        [Fact]
        public async Task Customers_Should_Be_Added_To_Outbox()
        {
            //Arrange
            var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c =>
                    c.GetAllAsync<BusinessCentralCustomer>(It.IsAny<string>(), It.IsAny<int>(), 
                        It.IsAny<CancellationToken>(),It.IsAny<bool>()))
                .ReturnsAsync(() => new List<BusinessCentralCustomer>() { Any.BusinessCentralCustomer() });
            var logger = NullLogger<BusinessCentralCustomerService>.Instance;
            var service = new BusinessCentralCustomerService(apiClientMock.Object, logger);

            //Act
            var customers = await service.GetAllAsync(CancellationToken.None);

            //Assert
            customers.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Default_Data_Initializer_Should_Put_Customers_In_Outbox()
        {
            // Arrange
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceLoggerMock = new Mock<ILogger<CustomerDefaultDataProvider>>();

            serviceScopeFactoryMock.Setup(c => c.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(c => c.ServiceProvider).Returns(serviceProviderMock.Object);
            var outbox = new FakeOutbox();
            var unitOfWork = new FakeUnitOfWork();
            serviceProviderMock.Setup(c => c.GetService(typeof(IOutbox))).Returns(outbox);
            serviceProviderMock.Setup(c => c.GetService(typeof(IUnitOfWork))).Returns(unitOfWork);

            var customers = new List<BusinessCentralCustomer>()
            {
                Any.BusinessCentralCustomer()
            };

            var customerService = TestHelpers.CustomerServiceFactory(customers);

            serviceProviderMock.Setup(_ => _.GetService(typeof(BusinessCentralCustomerService)))
                .Returns(customerService);

            var sut = new CustomerDefaultDataProvider(serviceScopeFactoryMock.Object, serviceLoggerMock.Object);
            var documentStoreMock = new Mock<IDocumentStore>();

            //Act
            await sut.Populate(documentStoreMock.Object, CancellationToken.None);

            //Assert
            outbox.Messages.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Customers_should_be_added_to_inbox()
        {
            //Arrange
            var inbox = new FakeInbox();
            var handler = new CustomerCreatedOrUpdated(inbox);

            //Act
            var integrationEvent = CreateCustomerIntegrationEvent();
            await handler.Handle(integrationEvent, CancellationToken.None);

            //Assert
            var inboxMessages = inbox.Messages;
            inboxMessages.Should().NotBeEmpty().And.HaveCount(1);
        }

        private static CustomerCreatedOrUpdatedIntegrationEvent CreateCustomerIntegrationEvent()
        {
            return CustomerCreatedOrUpdatedIntegrationEvent.Create(
                Any.Instance<Guid>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<decimal>(),
                Any.Instance<bool>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<decimal>(),
                Any.Instance<decimal>(),
                Any.Instance<string>(),
                Any.Instance<decimal>(),
                Any.Instance<string>(),
                Any.Instance<string>(),
                Any.Instance<string>());
        }
    }
}