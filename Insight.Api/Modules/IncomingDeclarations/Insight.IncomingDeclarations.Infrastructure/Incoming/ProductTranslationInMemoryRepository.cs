using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class ProductTranslationInMemoryRepository : InMemoryRepository<ProductTranslation>,
        IProductTranslationRepository
    {
        public static ProductTranslationInMemoryRepository CreateWithSeededData()
        {
            var result = new ProductTranslationInMemoryRepository();
            var dataProvider = new DefaultProductTranslationProvider(new NullServiceScopeFactory());
            dataProvider.CreateProductTranslationAsync(result).GetAwaiter().GetResult();

            return result;
        }

        public async Task<IReadOnlyList<string>> GetProductVariantsAsync(CancellationToken cancellationToken = default)
        {
            var result = Query().SelectMany(e => e.ProductVariants).Select(e => e.Value);
            return await IQueryableExtension.ToMartenListAsync(result, cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask; // This line is to simulate an asynchronous operation
            return false;
        }

        public async Task<IReadOnlyList<ProductTranslation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query(), cancellationToken);
        }
    }    
}