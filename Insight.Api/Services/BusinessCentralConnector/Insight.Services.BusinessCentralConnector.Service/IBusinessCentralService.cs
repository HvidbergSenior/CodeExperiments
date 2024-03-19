namespace Insight.Services.BusinessCentralConnector.Service;

public interface IBusinessCentralService<T>
{
    public Task<IEnumerable<T>> GetAllTransactionsAfterDateAsync(DateTimeOffset fromDate, CancellationToken cancellationToken);


    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}