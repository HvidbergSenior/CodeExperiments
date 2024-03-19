using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Services.AllocationEngine.Domain;
using Marten;

namespace Insight.Services.AllocationEngine.Infrastructure
{
    public class AllocationDraftRepository : MartenDocumentRepository<AllocationDraft>, IAllocationDraftRepository
    {
        public AllocationDraftRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            
        }
        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }
    }
}
