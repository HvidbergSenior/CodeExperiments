using Insight.Services.BusinessCentralConnector.Service;

namespace Insight.Services.BusinessCentralConnector.Service
{
    public class BusinessCentralEntity : IBusinessCentralEntity
    {
        public Guid CompanyId { get; private set; } = Guid.Empty;

        public string CompanyName { get; private set; } = string.Empty; 

        public void SetCompanyId(Guid companyId)
        {
            CompanyId = companyId;
        }

        public void SetCompanyName(string companyName)
        {
            CompanyName = companyName;
        }
    }
}
