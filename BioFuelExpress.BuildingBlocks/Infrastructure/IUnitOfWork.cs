namespace BioFuelExpress.BuildingBlocks.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken);
    }
}
