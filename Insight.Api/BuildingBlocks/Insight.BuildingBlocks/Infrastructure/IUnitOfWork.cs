namespace Insight.BuildingBlocks.Infrastructure
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken);
        /// <summary>
        /// Do you know what you're doing?
        /// https://martendb.io/documents/sessions.html#ejecting-documents-from-a-session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void EjectAllOfType<T>();
    }
}
