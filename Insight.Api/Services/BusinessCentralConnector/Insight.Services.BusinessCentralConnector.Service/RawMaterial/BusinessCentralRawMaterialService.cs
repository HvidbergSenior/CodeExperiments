namespace Insight.Services.BusinessCentralConnector.Service.RawMaterial
{
    public class BusinessCentralRawMaterialService : BusinessCentralService<BusinessCentralRawMaterial>
    {
        private const string ENTITY_NAME = "insight_AllowedFeedstock";
        private const int PAGE_SIZE = 500;

        public BusinessCentralRawMaterialService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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
            return false;
        }
    }
}
