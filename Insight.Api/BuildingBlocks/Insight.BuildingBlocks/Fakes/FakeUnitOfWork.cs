using Insight.BuildingBlocks.Infrastructure;

namespace Insight.BuildingBlocks.Fakes
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public bool IsCommitted { get; private set; }

        public FakeUnitOfWork()
        {
            IsCommitted = false;
        }

        public Task Commit()
        {
            return Commit(CancellationToken.None);
        }

        public Task Commit(CancellationToken cancellationToken)
        {
            IsCommitted = true;
            return Task.CompletedTask;
        }

        public void EjectAllOfType<T>()
        {
            throw new NotImplementedException();
        }
    }
}