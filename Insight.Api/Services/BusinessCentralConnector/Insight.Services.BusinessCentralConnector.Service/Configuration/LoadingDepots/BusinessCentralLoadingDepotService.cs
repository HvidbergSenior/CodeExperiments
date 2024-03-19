namespace Insight.Services.BusinessCentralConnector.Service.Configuration.LoadingDepots
{
    public class BusinessCentralLoadingDepotService : BusinessCentralService<BusinessCentralLoadingDepot>
    {
        private const string ENTITY_NAME = "insight_LoadingDepots";
        private const int PAGE_SIZE = 500;
        public BusinessCentralLoadingDepotService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
        {
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
