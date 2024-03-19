using Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsDialog
{
    public class BusinessCentralFuelTransactionsDialogService : BusinessCentralService<BusinessCentralFuelTransactionsDialog>
    {
        private const string ENTITY_NAME = "insight_TransactionsDialog";
        private const int PAGE_SIZE = 1000;

        public BusinessCentralFuelTransactionsDialogService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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