using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Configuration;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service.TransactionsDialog;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insight.Services.BusinessCentralConnector.Test.BusinessCentralService
{
    public class BusinessCentralServiceTest
    {
        [Fact (Skip = "Don't spam Business Central")]
        public async Task Customers_should_be_received_from_business_central()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();
            
            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
                new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            var entityName = "insight_Customer";
            var pageSize = 100000;
            var isGlobalEndpoint = false;
            //Act
            var customers = await sut.GetAllAsync<BusinessCentralCustomer>(entityName, pageSize, CancellationToken.None, isGlobalEndpoint);
            //Assert
            customers.Should().NotBeEmpty();
        }
        
        [Fact (Skip = "Don't spam Business Central")]
        public async Task TransactionsTapnet_should_be_called()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();

            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
                new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            const string entityName = "insight_TransactionsTapnet";
            const int pageSize = 100000;
            const bool isGlobalEndpoint = true;
            var targetDateTime = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            //Act
            var transactions = await sut.GetAllTransactionsAfterDateAsync<BusinessCentralFuelTransactionsTapnet>(targetDateTime, entityName, pageSize, CancellationToken.None,isGlobalEndpoint);

            //Assert
            transactions.Should().NotBeEmpty();

        }
        
        [Fact (Skip = "Don't spam Business Central")]
        public async Task TransactionsDialog_should_be_called()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();

            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
                new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            const string entityName = "insight_TransactionsDialog";
            const int pageSize = 9000;
            const bool isGlobalEndpoint = true;
            var targetDateTime = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            //Act
            var transactions = await sut.GetAllTransactionsAfterDateAsync<BusinessCentralFuelTransactionsDialog>(targetDateTime, entityName, pageSize,  CancellationToken.None, isGlobalEndpoint);

            //Assert
            transactions.Should().NotBeEmpty();

        }
        [Fact (Skip = "Don't spam Business Central")]
        public async Task TransactionsTokheim_should_be_called()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();

            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
                new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            const string entityName = "insight_TransactionsTokheim";
            const int pageSize = 100000;
            const bool isGlobalEndpoint = true;
            var targetDateTime = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            //Act
            var transactions = await sut.GetAllTransactionsAfterDateAsync<BusinessCentralTransactionsTokheim>(targetDateTime, entityName, pageSize, CancellationToken.None, isGlobalEndpoint);

            //Assert
            transactions.Should().NotBeEmpty();

        }

        [Fact(Skip = "Don't spam Business Central")]
        public async Task TransactionsTapnet_should_be_called2()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();

            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
                new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            const string entityName = "insight_TransactionsTapnet";
            const int pageSize = 500;
            const bool isGlobalEndpoint = true;
            var targetDateTime = new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            //Act
            var transactions = await sut.GetTransactionsAfterDateByPageSizeAsync<BusinessCentralFuelTransactionsTapnet>(targetDateTime, pageSize, entityName, 0, CancellationToken.None,isGlobalEndpoint);

            //Assert
            (transactions.Count() <= pageSize).Should().BeTrue();
        }
    }
}