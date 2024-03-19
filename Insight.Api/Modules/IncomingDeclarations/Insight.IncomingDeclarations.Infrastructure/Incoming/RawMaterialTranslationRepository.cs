using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class RawMaterialTranslationRepository : MartenDocumentRepository<RawMaterialTranslation>,
        IRawMaterialTranslationRepository
    {
        private readonly IDocumentSession documentSession;

        public RawMaterialTranslationRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher ) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task<IReadOnlyList<string>> GetRawMaterialVariantsAsync(CancellationToken cancellationToken = default)
        {
            /*
            var result = Query().SelectMany(e => e.RawMaterialVariants).Select(e => e.Value);
            return await IQueryableExtension.ToListAsync(result, cancellationToken);
            */

            // TOOD: JDO: Der skal IKKE være ToList() i vores repository... og slet ikke 2 gange ToList()
            var result_that_works = Query().ToList().SelectMany(e => e.RawMaterialVariants).Select(e => e.Value).ToList();
            await Task.CompletedTask;
            return result_that_works;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RawMaterialTranslation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await IQueryableExtension.ToMartenListAsync(Query(), cancellationToken);
        }
    }
}