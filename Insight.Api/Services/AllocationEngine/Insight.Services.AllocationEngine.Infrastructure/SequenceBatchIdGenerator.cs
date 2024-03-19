using Insight.Services.AllocationEngine.Domain;
using Marten;
using Npgsql;

namespace Insight.Services.AllocationEngine.Infrastructure;

public class SequenceBatchIdGenerator : ISequenceBatchIdGenerator
{
    private readonly IDocumentSession _documentSession;

    public SequenceBatchIdGenerator(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
 
    public async Task<long> GetNextBatchId(CancellationToken cancellationToken)
    {
        await using var sequenceCommand = new NpgsqlCommand("CREATE SEQUENCE IF NOT EXISTS BATCH_ID_SEQUENCE START 11111111");
        await _documentSession.ExecuteAsync(sequenceCommand, cancellationToken);
        await using var nextValCommand = new NpgsqlCommand("SELECT nextval('BATCH_ID_SEQUENCE');");
        await using var reader = await _documentSession.ExecuteReaderAsync(nextValCommand, cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return reader.GetInt64(0);
        }

        throw new InvalidOperationException("Not able to generate next project number");
    }
}