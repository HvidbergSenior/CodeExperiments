using Insight.BuildingBlocks.Domain;
using Marten;

namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public class MartenReadOnlyRepository<T> : IReadonlyRepository<T> where T : Entity
    {
        private readonly IQuerySession _querySession;

        public MartenReadOnlyRepository(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public IQueryable<T> Query()
        {
            return _querySession.Query<T>();
        }
    }
}
