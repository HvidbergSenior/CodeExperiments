namespace Insight.Services.BusinessCentralConnector.Service.Co2Target
{
    public class BusinessCentralCo2TargetService : BusinessCentralService<BusinessCentralCo2Target>
    {
        private const string ENTITY_NAME = "insight_Co2Target";
        private const int PAGE_SIZE = 500;

        public BusinessCentralCo2TargetService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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