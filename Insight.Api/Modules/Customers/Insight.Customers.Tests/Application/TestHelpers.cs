using Insight.Customers.Application.AllowedRawMaterials;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Insight.Customers.Tests.Application
{
    public static class TestHelpers
    {
        public static BusinessCentralCustomerService CustomerServiceFactory(List<BusinessCentralCustomer> customersToReturn)
        {
           var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c => c.GetAllAsync<BusinessCentralCustomer>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(),It.IsAny<bool>()))
                .ReturnsAsync(customersToReturn);
            var logger = NullLogger<BusinessCentralCustomerService>.Instance;
            var service = new BusinessCentralCustomerService(apiClientMock.Object, logger);
            
            return service;
        }

        public static BusinessCentralCo2TargetService CO2TargetServiceFactory(List<BusinessCentralCo2Target> co2TargetsToReturn)
        {
            var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c => c.GetAllAsync<BusinessCentralCo2Target>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(co2TargetsToReturn);

            var service = new BusinessCentralCo2TargetService(apiClientMock.Object);

            return service;
        }

        public static BusinessCentralRawMaterialService AllowedRawMaterialsServiceFactory(List<BusinessCentralRawMaterial> rawMaterialsToReturn)
        {
            var apiClientMock = new Mock<IBusinessCentralApiClient>();
            apiClientMock.Setup(c => c.GetAllAsync<BusinessCentralRawMaterial>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync(rawMaterialsToReturn);

            var service = new BusinessCentralRawMaterialService(apiClientMock.Object);

            return service;
        }
    }
}
