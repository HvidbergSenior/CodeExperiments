using Insight.BuildingBlocks.Infrastructure;

namespace Insight.BusinessCentralEntities.Domain.Products
{
    public interface IProductRepository : IRepository<Product>, IReadonlyRepository<Product>
    {
        public Task<IEnumerable<ItemCategoryCode>> GetItemCategoryCodes(CancellationToken cancellationToken = default);
        public Task<Product?> GetProductBySystemId(SourceSystemId sourceSystemId, CancellationToken cancellationToken = default);
        public Task<Product?> GetProductByProductNumberAndCompanyIdAsync(ProductNumber productNumber, CompanyId companyId, CancellationToken cancellationToken = default);
    }
}
