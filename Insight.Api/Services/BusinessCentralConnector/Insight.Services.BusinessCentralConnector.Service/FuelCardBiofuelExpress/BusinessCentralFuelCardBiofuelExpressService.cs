namespace Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress
{
    public class BusinessCentralFuelCardBiofuelExpressService : BusinessCentralService<BusinessCentralFuelCardBiofuelExpress>
    {
        private const string ENTITY_NAME = "insight_FuelCardBiofuelExpress";
        private const int PAGE_SIZE = 1000;

        public BusinessCentralFuelCardBiofuelExpressService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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
