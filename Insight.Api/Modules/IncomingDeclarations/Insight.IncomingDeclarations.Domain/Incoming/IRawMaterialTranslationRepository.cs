using Insight.BuildingBlocks.Infrastructure;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public interface IRawMaterialTranslationRepository : IRepository<RawMaterialTranslation>,
        IReadonlyRepository<RawMaterialTranslation>
    {
        Task<IReadOnlyList<RawMaterialTranslation>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<string>> GetRawMaterialVariantsAsync(CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}