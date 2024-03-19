using FluentAssertions;
using Insight.Services.BusinessCentralConnector.Service.Configuration;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insight.Services.BusinessCentralConnector.Test.BusinessCentralAuthentication
{
    public class BusinessCentralAuthenticationTest
    {
        [Fact (Skip = "Don't spam Business Central")]
        public async void Authentication_should_work()
        {
            //Arrange
            var options = new BusinessCentralOptions();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            httpClientFactoryMock.Setup(c=> c.CreateClient(It.IsAny<string>())).Returns(client);
            var authLogger = new Mock<ILogger<BusinessCentralAuthHelper>>();
            var authHelper = new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLogger.Object);
            
            //Act
            var accessToken = await authHelper.GetAccessTokenAsync();
            
            //Assert
            accessToken.Should().NotBeNull();
        }
        
        [Fact (Skip = "Don't spam Business Central")]
        public async Task Authentication_should_throw_exception()
        {
            //Arrange
            var options = new BusinessCentralOptions
            {
                ClientId = "56d0b3ac-2eb9-4f8c-9240-4554719573a3"
            };
            
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var client = new HttpClient();
            httpClientFactoryMock.Setup(c=> c.CreateClient(It.IsAny<string>())).Returns(client);
            var authLogger = new Mock<ILogger<BusinessCentralAuthHelper>>();

            var authHelper = new BusinessCentralAuthHelper(options, httpClientFactoryMock.Object, authLogger.Object);

            //Act and Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await authHelper.GetAccessTokenAsync());
        }
    }
}