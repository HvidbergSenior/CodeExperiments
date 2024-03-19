namespace Insight.Services.BusinessCentralConnector.Service.Product
{
    public class BusinessCentralProductService : BusinessCentralService<BusinessCentralProduct>
    {
        private const string ENTITY_NAME = "insight_Product";
        private const int PAGE_SIZE = 500;
        private readonly IBusinessCentralApiClient businessCentralApiClient;

        public BusinessCentralProductService(IBusinessCentralApiClient businessCentralApiClient) : base(businessCentralApiClient)
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
            return false;
        }

        public async Task<BusinessCentralProduct?> GetProductByProductAndCompanyIdNumber(string productNumber, Guid companyId, string companyName, CancellationToken cancellationToken)
        {
            var customQuery = $"$filter=no eq '{productNumber}'";

            var products = await businessCentralApiClient.GetItemsByCustomQueryAndCompanyAsync<BusinessCentralProduct>(GetEntityName(), GetPageSize(), customQuery, companyId, companyName, cancellationToken, false);
            
            var categoryCodeGrouping = products.GroupBy(x => x.ItemCategoryCode);
            
            if(categoryCodeGrouping.Count() > 1)
            {
                throw new PlatformNotSupportedException($"More than one product found with product number {productNumber}");
            }
            return categoryCodeGrouping.FirstOrDefault()?.FirstOrDefault();
        }

    }
}