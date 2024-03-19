using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Insight.Services.BusinessCentralConnector.Service.Customer
{
    public class BusinessCentralCustomerService : BusinessCentralService<BusinessCentralCustomer>
    {
        private const string ENTITY_NAME = "insight_Customer";
        private const int PAGE_SIZE = 500;
        private readonly IBusinessCentralApiClient businessCentralApiClient;
        private readonly ILogger<BusinessCentralCustomerService> logger;

        public BusinessCentralCustomerService(IBusinessCentralApiClient businessCentralApiClient, ILogger<BusinessCentralCustomerService> logger) : base(businessCentralApiClient)
        {
            this.businessCentralApiClient = businessCentralApiClient;
            this.logger = logger;
        }

        public override string GetEntityName()
        {
            return ENTITY_NAME;
        }

        public override int GetPageSize()
        {
            return PAGE_SIZE;
        }
        public override bool IsGlobalEndpoint()
        {
            return false;
        }

        public async Task<BusinessCentralCustomer?> GetCustomerByCustomerNumber(string customerNumber, CancellationToken cancellationToken)
        {
            var customQuery = $"$filter=no eq '{customerNumber}'";

            var customers = await businessCentralApiClient.GetItemsByCustomQueryAsync<BusinessCentralCustomer>(GetEntityName(), GetPageSize(), customQuery, cancellationToken, false);
            if(customers.Count() > 1)
            {
                logger.LogWarning("More than one customer found with customer number {CustomerNumber}", customerNumber);
                //throw new PlatformNotSupportedException($"More than one customer found with customer number {customerNumber}");
            }
            return customers.FirstOrDefault();
        }

        public async Task<BusinessCentralCustomer?> GetCustomerByCustomerNumberAndCompany(string customerNumber, Guid companyId, string companyName, CancellationToken cancellationToken)
        {
            var customQuery = $"$filter=no eq '{customerNumber}'";

            var customers = await businessCentralApiClient.GetItemsByCustomQueryAndCompanyAsync<BusinessCentralCustomer>(GetEntityName(), GetPageSize(), customQuery, companyId, companyName, cancellationToken, false);
            if (customers.Count() > 1)
            {
                logger.LogWarning("More than one customer found with customer number {CustomerNumber}", customerNumber);
                //throw new PlatformNotSupportedException($"More than one customer found with customer number {customerNumber}");
            }
            return customers.FirstOrDefault();
        }

    }
}