namespace Insight.BusinessCentralEntities.Integration.GetProductByNumber
{
    public class ProductDto
    {
        public string ProductNumber { get; private set; } = string.Empty;
        public string ProductName { get; private set; } = string.Empty;
        public string CompanyName { get; private set; } = string.Empty;
        public Guid CompanyId { get; private set; } = Guid.Empty;

        public ProductDto(string productNumber, string productName, Guid companyId, string companyName)
        {
            ProductNumber = productNumber;
            ProductName = productName;
            CompanyId = companyId;
            CompanyName = companyName;
        }
    }
}
