using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.Services.AllocationEngine.Domain;
using Marten;

namespace Insight.Services.AllocationEngine.Infrastructure
{
    public class DefaultAllocationDraftSeeder : IDefaultDataProvider
    {
        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using(var session = documentStore.IdentitySession())
            {
                var allocationDraft = await session.Query<AllocationDraft>().FirstOrDefaultAsync(cancellation);
                if(allocationDraft == null)
                {
                    allocationDraft = AllocationDraft.Create();
                    session.Store(allocationDraft);
                    await session.SaveChangesAsync(cancellation);
                }
            }
        }
    }
}
