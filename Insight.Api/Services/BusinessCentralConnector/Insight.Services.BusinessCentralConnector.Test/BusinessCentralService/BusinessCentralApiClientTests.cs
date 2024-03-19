using FluentAssertions;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Insight.Services.BusinessCentralConnector.Service.Configuration;

namespace Insight.Services.BusinessCentralConnector.Test.BusinessCentralService
{
    public class BusinessCentralApiClientTests
    {
        [Fact(Skip ="Don't spam BC")]
        public async Task Make_Sure_We_Can_Call_BC()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var helper = new BusinessCentralUrlHelper(options);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var authLoggerMock = new Mock<ILogger<BusinessCentralAuthHelper>>();
            var serviceLoggerMock = new Mock<ILogger<BusinessCentralService<BusinessCentralCustomer>>>();

            var client = new HttpClient();
            var client2 = new HttpClient();
            httpClientFactoryMock.SetupSequence(c => c.CreateClient(It.IsAny<string>()))
                .Returns(client)
                .Returns(client2);

            var authHelper =
               new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLoggerMock.Object);

            var sut = new BusinessCentralApiClient(httpClientFactoryMock.Object, authHelper, helper);

            var entityName = "insight_Customer";
            var pageSize = 500;
            var isGlobalEndpoint = false;
            //Act
            var customers = await sut.GetAllAsync<BusinessCentralCustomer>(entityName, pageSize,  CancellationToken.None, isGlobalEndpoint);

            //Assert
            customers.Should().NotBeEmpty();
        }
    }
}
