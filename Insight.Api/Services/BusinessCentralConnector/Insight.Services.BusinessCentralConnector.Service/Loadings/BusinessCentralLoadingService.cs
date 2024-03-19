namespace Insight.Services.BusinessCentralConnector.Service.Loadings
{
    public class BusinessCentralLoadingService : BusinessCentralService<BusinessCentralLoading>
    {
        private const string ENTITY_NAME = "insight_Loadings";
        private const int PAGE_SIZE = 1000;
        private readonly IBusinessCentralApiClient businessCentralApiClient;

        public BusinessCentralLoadingService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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
            return true;
        }
    }
}
