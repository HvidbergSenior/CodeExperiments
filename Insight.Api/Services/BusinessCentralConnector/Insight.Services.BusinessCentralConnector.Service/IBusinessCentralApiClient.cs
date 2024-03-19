using Insight.Services.BusinessCentralConnector.Service.Company;

namespace Insight.Services.BusinessCentralConnector.Service
{
    public interface IBusinessCentralApiClient
    {
        Task<IEnumerable<T>> GetAllTransactionsAfterDateAsync<T>(DateTimeOffset fromDate, string entityName, int pageSize, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<IEnumerable<T>> GetAllAsync<T>(string entityName, int pageSize,  CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<IEnumerable<T>> GetTransactionsAfterDateByPageSizeAsync<T>(DateTimeOffset fromDate, int pageSize, string entityName, int transactionsToSkip, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<IEnumerable<T>> GetTransactionsByPageSizeAsync<T>(int pageSize, string entityName, int transactionsToSkip, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<IEnumerable<T>> GetItemsByCustomQueryAsync<T>(string entityName, int pageSize, string oDataQuery, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<IEnumerable<T>> GetItemsByCustomQueryAndCompanyAsync<T>(string entityName, int pageSize, string oDataQuery, Guid companyId, string companyName, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity;
        Task<BusinessCentralCompany[]> GetCompaniesAsync<T>(bool isGlobalEndpoint, CancellationToken cancellationToken);
    }
}
