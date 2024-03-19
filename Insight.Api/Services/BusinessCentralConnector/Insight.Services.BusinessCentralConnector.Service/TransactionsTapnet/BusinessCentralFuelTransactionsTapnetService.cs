using Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet
{
    public class BusinessCentralFuelTransactionsTapnetService : BusinessCentralService<BusinessCentralFuelTransactionsTapnet>
    {
        private const string ENTITY_NAME = "insight_TransactionsTapnet";
        private const int PAGE_SIZE = 500;

        public BusinessCentralFuelTransactionsTapnetService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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