using Insight.BuildingBlocks.Application.Queries;

namespace Insight.BusinessCentralEntities.Integration.GetProductByNumber
{
    public sealed class GetProductByNumberQuery : IQuery<ProductDto>
    {
        public string ProductNumber { get; private set; }
        public Guid CompanyId { get; private set; }

        private GetProductByNumberQuery(string productNumber, Guid companyId)
        {
            ProductNumber = productNumber;
            CompanyId = companyId;
        }

        public static GetProductByNumberQuery Create(string productNumber, Guid companyId)
        {
            return new GetProductByNumberQuery(productNumber, companyId);
        }
    }
}
