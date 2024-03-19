namespace Insight.Services.BusinessCentralConnector.Service;

public abstract class BusinessCentralService<T> : IBusinessCentralService<T> where T : BusinessCentralEntity
{
    private readonly IBusinessCentralApiClient businessCentralApiClient;

    public BusinessCentralService(IBusinessCentralApiClient businessCentralApiClient)
    {
        this.businessCentralApiClient = businessCentralApiClient;
    }

    public abstract string GetEntityName();
    
    public abstract int GetPageSize();

    public abstract bool IsGlobalEndpoint();

    public async Task<IEnumerable<T>> GetAllTransactionsAfterDateAsync(DateTimeOffset fromDate, CancellationToken cancellationToken)
    {
        return await businessCentralApiClient.GetAllTransactionsAfterDateAsync<T>(fromDate, GetEntityName(), GetPageSize(), cancellationToken, IsGlobalEndpoint()).ConfigureAwait(false);
    }
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await businessCentralApiClient.GetAllAsync<T>(GetEntityName(), GetPageSize(), cancellationToken, IsGlobalEndpoint()).ConfigureAwait(false);
    }
    public async Task<IEnumerable<T>> GetTransactionsAfterDateByPagingAsync(DateTimeOffset fromDate, int transactionsToGet, int transactionsToSkip, CancellationToken cancellationToken)
    {
        return await businessCentralApiClient.GetTransactionsAfterDateByPageSizeAsync<T>(fromDate, transactionsToGet, GetEntityName(), transactionsToSkip, cancellationToken, IsGlobalEndpoint()).ConfigureAwait(false);
    }
    // Todo: Temporary until they have added timestamps to item ledger
    public async Task<IEnumerable<T>> GetTransactionsByPagingAsync(int transactionsToGet, int transactionsToSkip, CancellationToken cancellationToken)
    {
        return await businessCentralApiClient.GetTransactionsByPageSizeAsync<T>(transactionsToGet, GetEntityName(), transactionsToSkip, cancellationToken, IsGlobalEndpoint()).ConfigureAwait(false);
    }
}