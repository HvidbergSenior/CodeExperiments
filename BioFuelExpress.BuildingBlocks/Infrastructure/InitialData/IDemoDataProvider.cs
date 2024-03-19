using Marten;

namespace BioFuelExpress.BuildingBlocks.Infrastructure.InitialData
{
    public interface IDemoDataProvider
    {
        Task Populate(IDocumentStore documentStore, CancellationToken cancellation);
    }
}
