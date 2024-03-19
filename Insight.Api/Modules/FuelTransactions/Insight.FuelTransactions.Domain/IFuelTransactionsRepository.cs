using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;

namespace Insight.FuelTransactions.Domain;

public interface IFuelTransactionsRepository : IRepository<FuelTransaction>, IReadonlyRepository<FuelTransaction>
{
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetExistingHashesAsync(string[] itemHashes, CancellationToken cancellationToken = default);
    Task<IEnumerable<FuelTransaction>> GetFuelTransactionsBetweenDates(FuelTransactionsBetweenDates fuelTransactionsBetweenDates, CancellationToken cancellationToken = default);
    Task<(IEnumerable<FuelTransaction> Transactions, int TotalCount)> GetFuelTransactionsRefinedAsync(FuelTransactionsBetweenDates fuelTransactionsBetweenDates,
        IEnumerable<ProductName> productNames,
        IEnumerable<FuelTransactionCustomerId> customerIds,
        PaginationParameters paginationParameters,
        SortingParameters sortingParameters,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ProductName>> GetProductNamesForCustomerId(FuelTransactionCustomerId customerId);

    Task<IEnumerable<CustomerName>> GetCustomerNamesForCustomerId(FuelTransactionCustomerId customerId);

    Task<IEnumerable<GroupedFuelTransaction>> GetGroupedTransactions(
        DatePeriod datePeriod,
        IEnumerable<ProductName> productNames,
        IEnumerable<CustomerId> customerIds,
        CancellationToken cancellationToken = default);
}