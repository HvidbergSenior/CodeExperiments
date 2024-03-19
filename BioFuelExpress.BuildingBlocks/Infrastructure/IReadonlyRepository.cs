namespace BioFuelExpress.BuildingBlocks.Infrastructure
{
    public interface IReadonlyRepository<out TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
    }
}
