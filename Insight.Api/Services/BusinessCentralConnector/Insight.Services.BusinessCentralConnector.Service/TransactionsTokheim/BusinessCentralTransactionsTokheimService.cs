namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim
{
    public class BusinessCentralFuelTransactionsTokheimService : BusinessCentralService<BusinessCentralTransactionsTokheim>
    {
        private const string ENTITY_NAME = "insight_TransactionsTokheim";
        private const int PAGE_SIZE = 1000;

        public BusinessCentralFuelTransactionsTokheimService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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