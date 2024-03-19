namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public interface IIncomingDeclarationAllocationRepository
    {
        public Task ClearDraftAllocationAsync(Dictionary<IncomingDeclarationId, List<Guid>> allocationAssignments, CancellationToken cancellationToken);
    }
}
