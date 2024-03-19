using Insight.BuildingBlocks.Infrastructure;

namespace Insight.Services.AllocationEngine.Domain
{
    public interface IAllocationDraftRepository : IRepository<AllocationDraft>, IReadonlyRepository<AllocationDraft>
    {
    }
}
