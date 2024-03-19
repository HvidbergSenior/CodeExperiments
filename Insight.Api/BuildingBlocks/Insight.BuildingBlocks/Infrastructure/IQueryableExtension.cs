using Marten;
using Marten.Linq;

namespace Insight.BuildingBlocks.Infrastructure
{
    public static class IQueryableExtension
    {
        public static Task<IReadOnlyList<T>> ToMartenListAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken)
        {
            if (queryable is IMartenQueryable<T>)
            {
                // Use Marten extension method because the type matches
                return QueryableExtensions.ToListAsync(queryable, cancellationToken);
            }
            // Dont use Marten extension method, since type is not a Marten queryable
            return Task.FromResult<IReadOnlyList<T>>(queryable.ToList());
        }
    }
}
