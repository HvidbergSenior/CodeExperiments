using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Customers.Application.AllowedRawMaterials;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.Customers.Tests.Application;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Insight.Customers.Tests.Integration
{
    public class AllowedRawMaterialsFromBcIntegrationTest
    {
        [Fact]
        public async Task AllowedRawMaterials_Should_Be_Retrieved_From_Service()
        {
            //Arrange
            var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c =>
                    c.GetAllAsync<BusinessCentralRawMaterial>(It.IsAny<string>(), It.IsAny<int>(),
                        It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(() => new List<BusinessCentralRawMaterial>() { Any.BusinessCentralRawMaterial() });

            var service = new BusinessCentralRawMaterialService(apiClientMock.Object);

            //Act
            var allowedRawMaterials = await service.GetAllAsync(CancellationToken.None);

            //Assert
            allowedRawMaterials.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Default_Data_Initializer_Should_Put_AllowedRawMaterials_In_Outbox()
        {
            // Arrange
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceLoggerMock = new Mock<ILogger<AllowedRawMaterialDefaultDataProvider>>();

            serviceScopeFactoryMock.Setup(c => c.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(c => c.ServiceProvider).Returns(serviceProviderMock.Object);
            var outbox = new FakeOutbox();
            var unitOfWork = new FakeUnitOfWork();
            serviceProviderMock.Setup(c => c.GetService(typeof(IOutbox))).Returns(outbox);
            serviceProviderMock.Setup(c => c.GetService(typeof(IUnitOfWork))).Returns(unitOfWork);

            var allowedRawMaterials = new List<BusinessCentralRawMaterial>()
            {
                Any.BusinessCentralRawMaterial()
            };

            var allowedRawMaterialsService = TestHelpers.AllowedRawMaterialsServiceFactory(allowedRawMaterials);

            serviceProviderMock.Setup(_ => _.GetService(typeof(BusinessCentralRawMaterialService)))
                .Returns(allowedRawMaterialsService);

            var sut = new AllowedRawMaterialDefaultDataProvider(serviceScopeFactoryMock.Object, serviceLoggerMock.Object);
            var documentStoreMock = new Mock<IDocumentStore>();

            //Act
            await sut.Populate(documentStoreMock.Object, CancellationToken.None);

            //Assert
            outbox.Messages.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task AllowedRawMaterials_should_be_added_to_inbox()
        {
            //Arrange
            var inbox = new FakeInbox();
            var handler = new AllowedRawMaterialUpdated(inbox);

            //Act
            var integrationEvent = CreateAllowedRawMaterialIntegrationEvent();
            await handler.Handle(integrationEvent, CancellationToken.None);

            //Assert
            var inboxMessages = inbox.Messages;
            inboxMessages.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Customer_Should_Have_AllowedRawMaterials_Updated()
        {
            var repoMock = new Mock<ICustomerRepository>();
            var customer = Any.Customer();
            repoMock.Setup(c => c.GetByCustomerNumberAndCompanyIdAsync(It.IsAny<CustomerNumber>(), It.IsAny<CompanyId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var uOWMock = new Mock<IUnitOfWork>();

            var logger = NullLogger<AllowedRawMaterialUpdatedCommandHandler>.Instance;

            var handler = new AllowedRawMaterialUpdatedCommandHandler(uOWMock.Object, repoMock.Object, logger);

            var allowedRawMaterialsCommand = Any.AllowedRawMaterialUpdatedCommand();

            customer.AllowedRawMaterials.Value.Should().NotBeSameAs(allowedRawMaterialsCommand.AllowedRawMaterials.Value);
            
            await handler.Handle(allowedRawMaterialsCommand, CancellationToken.None);
            
            customer.AllowedRawMaterials.Value.Should().BeSameAs(allowedRawMaterialsCommand.AllowedRawMaterials.Value);
        }

        private static AllowedRawMaterialUpdatedIntegrationEvent CreateAllowedRawMaterialIntegrationEvent()
        {
            return AllowedRawMaterialUpdatedIntegrationEvent.Create(Any.Instance<Guid>(), Any.Instance<string>(), Any.Instance<Dictionary<string, bool>>());
        }
    }
}
