using BioFuelExpress.BuildingBlocks.Domain;
using Marten;

namespace BioFuelExpress.BuildingBlocks.Infrastructure.Marten
{
    public class MartenReadOnlyRespository<T> : IReadonlyRepository<T> where T : Entity
    {
        private readonly IQuerySession querySession;

        public MartenReadOnlyRespository(IQuerySession querySession)
        {
            this.querySession = querySession;
        }

        public IQueryable<T> Query()
        {
            return querySession.Query<T>();
        }
    }
}
