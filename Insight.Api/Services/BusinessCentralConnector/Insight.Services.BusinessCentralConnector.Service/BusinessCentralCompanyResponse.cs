using Insight.Services.BusinessCentralConnector.Service.Company;

namespace Insight.Services.BusinessCentralConnector.Service
{
    public class BusinessCentralCompanyResponse
    {
        public BusinessCentralCompany[] Value { get; set; } = Array.Empty<BusinessCentralCompany>();
    }
}
