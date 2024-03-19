namespace Insight.Services.BusinessCentralConnector.Service.FuelCardAcceptance
{
    public class BusinessCentralFuelCardAcceptanceService : BusinessCentralService<BusinessCentralFuelCardAcceptance>
    {
        private const string ENTITY_NAME = "insight_FuelCardCardAcceptance";
        private const int PAGE_SIZE = 500;

        public BusinessCentralFuelCardAcceptanceService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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