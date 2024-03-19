namespace Insight.Services.AllocationEngine.Domain
{
    public interface ISequenceBatchIdGenerator
    {
        Task<long> GetNextBatchId(CancellationToken cancellationToken);
    }
}
