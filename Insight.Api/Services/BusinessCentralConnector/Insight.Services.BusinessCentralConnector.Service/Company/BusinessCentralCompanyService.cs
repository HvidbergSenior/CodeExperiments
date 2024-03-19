namespace Insight.Services.BusinessCentralConnector.Service.Company
{
    public class BusinessCentralCompanyService : BusinessCentralService<BusinessCentralCompany>
    {
        private const string ENTITY_NAME = "insight_Companies";
        private const int PAGE_SIZE = 5;
        private readonly IBusinessCentralApiClient businessCentralApiClient;

        public BusinessCentralCompanyService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
        {
            this.businessCentralApiClient = businessCentralApiClient;
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

        public async Task<BusinessCentralCompany[]> GetCompanies(CancellationToken cancellationToken)
        {
            return await businessCentralApiClient.GetCompaniesAsync<BusinessCentralCompany>(false, cancellationToken);
        }
    }
}