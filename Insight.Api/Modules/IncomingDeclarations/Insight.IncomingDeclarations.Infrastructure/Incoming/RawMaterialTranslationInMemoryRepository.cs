using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class RawMaterialTranslationInMemoryRepository : InMemoryRepository<RawMaterialTranslation>,
        IRawMaterialTranslationRepository
    {
        public static RawMaterialTranslationInMemoryRepository CreateWithSeededData()
        {
            var result = new RawMaterialTranslationInMemoryRepository();
            var dataProvider = new DefaultRawMaterialTranslationProvider(new NullServiceScopeFactory());
            dataProvider.CreateRawMaterialTranslationAsync(result).GetAwaiter().GetResult();

            return result;
        }

        public async Task<IReadOnlyList<string>> GetRawMaterialVariantsAsync(CancellationToken cancellationToken = default)
        {
            var result = Query().SelectMany(e => e.RawMaterialVariants).Select(e => e.Value);
            return await IQueryableExtension.ToMartenListAsync(result, cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask; // This line is to simulate an asynchronous operation
            return false; 
        }

        public async Task<IReadOnlyList<RawMaterialTranslation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query(), cancellationToken);
        }
    }

    public class NullServiceScopeFactory : IServiceScopeFactory
    {
        public IServiceScope CreateScope()
        {
            throw new NotImplementedException();
        }
    }
}