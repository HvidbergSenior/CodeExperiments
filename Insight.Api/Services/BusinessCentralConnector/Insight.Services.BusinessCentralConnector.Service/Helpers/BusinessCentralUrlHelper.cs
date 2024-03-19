using Insight.Services.BusinessCentralConnector.Service.Configuration;
using System.Globalization;

namespace Insight.Services.BusinessCentralConnector.Service.Helpers
{
    public class BusinessCentralUrlHelper
    {
        private readonly IBusinessCentralConfig businessCentralConfig;

        public BusinessCentralUrlHelper(IBusinessCentralConfig businessCentralConfig)
        {
            this.businessCentralConfig = businessCentralConfig;
        }

        public string GetBusinessCentralCompaniesUrl()
        {
            return
                $"{businessCentralConfig.BaseUrl}/{businessCentralConfig.TenantId}/{businessCentralConfig.EnvironmentPath}/companies";
        }

        public string GetBusinessCentralFirstCompany()
        {
            return
                $"{businessCentralConfig.BaseUrl}/{businessCentralConfig.TenantId}/{businessCentralConfig.EnvironmentPath}/companies?$top=" +
                1;
        }

        public string GetBusinessCentralUrl(Guid companyId, string entityName)
        {
            return
                $"{businessCentralConfig.BaseUrl}/{businessCentralConfig.TenantId}/{businessCentralConfig.EnvironmentPath}/companies({companyId})/{entityName}";
        }

        public string GetBusinessCentralUrlWithPagination(Guid companyId, string entityName, int pageSize, int skip)
        {
            return
                $"{businessCentralConfig.BaseUrl}/{businessCentralConfig.TenantId}/{businessCentralConfig.EnvironmentPath}/companies({companyId})/{entityName}?$top=" +
                pageSize + "&$skip=" + skip;
        }

        public string GetBusinessCentralTransactionUrlWithPaginationAndFromDate(DateTimeOffset fromDateTime, Guid companyId,
            string entityName, int pageSize, int skip)
        {
            var comparator = "gt";
            if(fromDateTime == DateTimeOffset.MinValue)
            {
                comparator = "ge";
            }

            //lt = lesser than, gt = greater than, ge = greater or equal
            var filter = $"$filter=SystemCreatedAt {comparator} {fromDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture)}";
            // Skip the first "skip" entities. Show the next Top
            return $"{businessCentralConfig.BaseUrl}/{businessCentralConfig.TenantId}/{businessCentralConfig.EnvironmentPath}/companies({companyId})/{entityName}?$top={pageSize}&$skip={skip}&{filter}&orderby=SystemCreatedAt";
        }
    }
}