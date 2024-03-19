namespace Insight.Services.BusinessCentralConnector.Service.Itemledger
{
    public class BusinessCentralItemLedgerService : BusinessCentralService<BusinessCentralItemLedger>
    {
        private const string ENTITY_NAME = "insight_ItemLedger";
        private const int PAGE_SIZE = 1000;
        public BusinessCentralItemLedgerService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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
