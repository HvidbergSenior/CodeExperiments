using Marten;

namespace BioFuelExpress.BuildingBlocks.Infrastructure.Marten
{
    public class MartenUnitOfWork : IUnitOfWork
    {
        private readonly IDocumentSession documentSession;

        public MartenUnitOfWork(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await documentSession.SaveChangesAsync(cancellationToken);
        }
    }
}
