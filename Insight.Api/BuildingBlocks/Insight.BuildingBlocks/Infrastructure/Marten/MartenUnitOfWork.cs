using Marten;

namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public class MartenUnitOfWork : IUnitOfWork
    {
        private readonly IDocumentSession _documentSession;

        public MartenUnitOfWork(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await _documentSession.SaveChangesAsync(cancellationToken);
        }

        public void EjectAllOfType<T>()
        {
            _documentSession.EjectAllOfType(typeof(T));
        }
    }
}
