using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class ProductTranslationRepository : MartenDocumentRepository<ProductTranslation>,
        IProductTranslationRepository
    {
        private readonly IDocumentSession documentSession;

        public ProductTranslationRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task<IReadOnlyList<string>> GetProductVariantsAsync(CancellationToken cancellationToken = default)
        {
            var result = await Query().SelectMany(c => c.ProductVariants).ToListAsync(cancellationToken);
            return result.Select(c => c.Value).ToList();
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ProductTranslation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Query().ToMartenListAsync(cancellationToken);
        }
    }
}