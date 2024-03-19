using Insight.BuildingBlocks.Infrastructure;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public interface IProductTranslationRepository : IRepository<ProductTranslation>, IReadonlyRepository<ProductTranslation>
    {
        Task<IReadOnlyList<ProductTranslation>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetProductVariantsAsync(CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}