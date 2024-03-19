using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Products;
using Marten;

namespace Insight.BusinessCentralEntities.Infrastructure
{
    public class ProductRepository : MartenDocumentRepository<Product>, IProductRepository
    {
        public ProductRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public async Task<IEnumerable<ItemCategoryCode>> GetItemCategoryCodes(CancellationToken cancellationToken = default)
        {
            return await Query().Select(p => p.ItemCategoryCode).Distinct().ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductByProductNumberAndCompanyIdAsync(ProductNumber productNumber, CompanyId companyId, CancellationToken cancellationToken = default)
        {
            return await Query().FirstOrDefaultAsync(p => p.ProductNumber.Value == productNumber.Value && p.CompanyId.Value == companyId.Value, cancellationToken);
        }

        public async Task<Product?> GetProductBySystemId(SourceSystemId sourceSystemId, CancellationToken cancellationToken)
        {
            return await Query().FirstOrDefaultAsync(p => p.SourceSystemId.Value == sourceSystemId.Value, cancellationToken);
        }
    }
}
