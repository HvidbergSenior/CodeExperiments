using Marten;

namespace Insight.BuildingBlocks.Infrastructure.InitialData
{
    public interface IDefaultDataProvider
    {
        Task Populate(IDocumentStore documentStore, CancellationToken cancellation);
    }
}
