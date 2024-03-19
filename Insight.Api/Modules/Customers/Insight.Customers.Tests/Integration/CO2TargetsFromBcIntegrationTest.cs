using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Customers.Application.CO2Targets;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.Customers.Tests.Application;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Insight.Customers.Tests.Integration
{
    public class CO2TargetsFromBcIntegrationTest
    {
        [Fact]
        public async Task CO2Targets_Should_Be_Retrieved_From_Service()
        {
            //Arrange
            var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c =>
                    c.GetAllAsync<BusinessCentralCo2Target>(It.IsAny<string>(), It.IsAny<int>(),
                        It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(() => new List<BusinessCentralCo2Target>() { Any.BusinessCentralCo2Target() });

            var service = new BusinessCentralCo2TargetService(apiClientMock.Object);

            //Act
            var co2Targets = await service.GetAllAsync(CancellationToken.None);

            //Assert
            co2Targets.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Default_Data_Initializer_Should_Put_CO2Targets_In_Outbox()
        {
            // Arrange
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceLoggerMock = new Mock<ILogger<CO2TargetDefaultDataProvider>>();

            serviceScopeFactoryMock.Setup(c => c.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(c => c.ServiceProvider).Returns(serviceProviderMock.Object);
            var outbox = new FakeOutbox();
            var unitOfWork = new FakeUnitOfWork();
            serviceProviderMock.Setup(c => c.GetService(typeof(IOutbox))).Returns(outbox);
            serviceProviderMock.Setup(c => c.GetService(typeof(IUnitOfWork))).Returns(unitOfWork);

            var co2Targets = new List<BusinessCentralCo2Target>()
            {
                Any.BusinessCentralCo2Target()
            };

            var co2TargetService = TestHelpers.CO2TargetServiceFactory(co2Targets);

            serviceProviderMock.Setup(_ => _.GetService(typeof(BusinessCentralCo2TargetService)))
                .Returns(co2TargetService);

            var sut = new CO2TargetDefaultDataProvider(serviceScopeFactoryMock.Object, serviceLoggerMock.Object);
            var documentStoreMock = new Mock<IDocumentStore>();

            //Act
            await sut.Populate(documentStoreMock.Object, CancellationToken.None);

            //Assert
            outbox.Messages.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task CO2Targets_should_be_added_to_inbox()
        {
            //Arrange
            var inbox = new FakeInbox();
            var handler = new CO2TargetUpdated(inbox);

            //Act
            var integrationEvent = CreateCO2TargetIntegrationEvent();
            await handler.Handle(integrationEvent, CancellationToken.None);

            //Assert
            var inboxMessages = inbox.Messages;
            inboxMessages.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async Task Customer_Should_Have_Co2Target_Updated()
        {
            var repoMock = new Mock<ICustomerRepository>();
            var customer = Any.Customer();
            repoMock.Setup(c => c.GetByCustomerNumberAndCompanyIdAsync(It.IsAny<CustomerNumber>(), It.IsAny<CompanyId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var uOWMock = new Mock<IUnitOfWork>();

            var handler = new CO2TargetUpdatedCommandHandler(repoMock.Object, uOWMock.Object);
            
            var co2TargetCommand = Any.CO2TargetUpdatedCommand();
            
            customer.CO2Target.Value.Should().NotBe(co2TargetCommand.CO2Target.Value);

            await handler.Handle(co2TargetCommand, CancellationToken.None);

            customer.CO2Target.Value.Should().Be(co2TargetCommand.CO2Target.Value);
        }

        private static CO2TargetUpdatedIntegrationEvent CreateCO2TargetIntegrationEvent()
        {
            return CO2TargetUpdatedIntegrationEvent.Create(Any.Instance<Guid>(), Any.Instance<string>(), Any.Instance<decimal>());
        }
    }
}
