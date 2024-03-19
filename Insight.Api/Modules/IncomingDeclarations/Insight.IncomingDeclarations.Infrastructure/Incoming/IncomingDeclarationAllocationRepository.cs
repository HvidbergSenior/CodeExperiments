using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;

namespace Insight.IncomingDeclarations.Infrastructure.Incoming
{
    public class IncomingDeclarationAllocationRepository : MartenDocumentRepository<IncomingDeclaration>, IIncomingDeclarationAllocationRepository
    {
        private readonly IDocumentSession documentSession;

        public IncomingDeclarationAllocationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
        }

        public async Task ClearDraftAllocationAsync(Dictionary<IncomingDeclarationId, List<Guid>> allocationAssignments, CancellationToken cancellationToken)
        {
            foreach(var allocation in allocationAssignments)
            {
                var incomingDeclaration = await documentSession.LoadAsync<IncomingDeclaration>(allocation.Key.Value, cancellationToken);
                if(incomingDeclaration == null)
                {
                    throw new NotFoundException($"IncomingDeclaration {allocation.Key.Value} not found");
                }
                foreach(var id in allocation.Value)
                {
                    incomingDeclaration.Allocations.Value.Remove(id);
                }
                documentSession.Update(incomingDeclaration);
            }
        }
    }
}
