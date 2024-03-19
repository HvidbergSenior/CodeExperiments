using Marten;

namespace Insight.BuildingBlocks.Infrastructure.InitialData
{
    public interface IDemoDataProvider
    {
        Task Populate(IDocumentStore documentStore, CancellationToken cancellation);
    }
}
