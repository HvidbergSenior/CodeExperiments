namespace Insight.BuildingBlocks.Application.Queries
{
    public interface IQueryBus
    {
        Task<TResponse> Send<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResponse>;
    }
}
