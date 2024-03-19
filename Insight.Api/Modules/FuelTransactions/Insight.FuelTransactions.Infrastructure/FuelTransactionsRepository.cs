using System.Globalization;
using System.Text;
using Dapper;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Serialization;
using Insight.FuelTransactions.Domain;
using Marten;
using Marten.Pagination;
using PaginationParameters = Insight.FuelTransactions.Domain.PaginationParameters;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;


namespace Insight.FuelTransactions.Infrastructure
{
    public class FuelTransactionsRepository : MartenDocumentRepository<FuelTransaction>, IFuelTransactionsRepository
    {
        private readonly IDocumentSession documentSession;
        private readonly string schemaName;

        public FuelTransactionsRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            this.documentSession = documentSession;
            schemaName = documentSession.DocumentStore.Options.DatabaseSchemaName!;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetExistingHashesAsync(string[] itemHashes, CancellationToken cancellationToken)
        {
            return await Query().Where(c=> c.ItemHash.In(itemHashes)).Select(c=> c.ItemHash).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductName>> GetProductNamesForCustomerId(FuelTransactionCustomerId customerId)
        {
            var query = Query()
                .Where(query => customerId.Value == query.CustomerId.Value)
                .Select(query => query.ProductName)
                .Distinct();

            var items = await query.ToListAsync();
            return items;
        }

        public async Task<IEnumerable<CustomerName>> GetCustomerNamesForCustomerId(FuelTransactionCustomerId customerId)
        {
            var query = Query()
                .Where(query => customerId.Value == query.CustomerId.Value)
                .Select(query => query.CustomerName)
                .Distinct();

            var items = await query.ToListAsync();
            return items;
        }

        public async Task<IEnumerable<FuelTransaction>> GetFuelTransactionsBetweenDates(FuelTransactionsBetweenDates fuelTransactionsBetweenDates,
            CancellationToken cancellationToken = default)
        {
            var fromDateTimeOffSet = new DateTimeOffset(fuelTransactionsBetweenDates.FromDate.Year, fuelTransactionsBetweenDates.FromDate.Month, fuelTransactionsBetweenDates.FromDate.Day, 0, 0, 0, TimeSpan.Zero);
            var dateTimeOffset = new DateTimeOffset(fuelTransactionsBetweenDates.ToDate.Year, fuelTransactionsBetweenDates.ToDate.Month, fuelTransactionsBetweenDates.ToDate.Day, 0, 0, 0, TimeSpan.Zero);

            var query = Query().Where(query => fromDateTimeOffSet <= query.FuelTransactionTimeStamp &&
                                               dateTimeOffset >= query.FuelTransactionTimeStamp );
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<(IEnumerable<FuelTransaction> Transactions, int TotalCount)> GetFuelTransactionsRefinedAsync(FuelTransactionsBetweenDates fuelTransactionsBetweenDates,
            IEnumerable<ProductName> productNames,
            IEnumerable<FuelTransactionCustomerId> customerIds,
            PaginationParameters paginationParameters,
            SortingParameters sortingParameters,
            CancellationToken cancellationToken = default)
        {
            var fromTimestamp = new DateTimeOffset(fuelTransactionsBetweenDates.FromDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
            var toTimestamp = new DateTimeOffset(fuelTransactionsBetweenDates.ToDate.ToDateTime(TimeOnly.MaxValue), TimeSpan.Zero);

            //Marten can't compare ValueObjects, let's help it a bit
            var productNamesValues = productNames.Select(p => p.Value).ToList();
            var customerIdsValues = customerIds.Select(c => c.Value).ToList();

            var query = Query()
                .Where(query => fromTimestamp <= query.FuelTransactionTimeStamp &&
                                toTimestamp >= query.FuelTransactionTimeStamp)
                .Where(query => customerIdsValues.IsEmpty() || (customerIdsValues.Contains(query.CustomerId.Value) || customerIdsValues.Contains(query.AccountCustomerId.Value)))
                .Where(query => productNamesValues.IsEmpty() || productNamesValues.Contains(query.ProductName.Value));

            query = sortingParameters.IsOrderDescending
                ? query.OrderByDescending(sortingParameters.OrderByProperty)
                : query.OrderBy(sortingParameters.OrderByProperty);

            var transactions = await query.ToPagedListAsync(paginationParameters.Page, paginationParameters.PageSize);
            return (transactions, (int)transactions.TotalItemCount);
        }

        public async Task<IEnumerable<GroupedFuelTransaction>> GetGroupedTransactions(
            DatePeriod datePeriod,
            IEnumerable<ProductName> productNames,
            IEnumerable<CustomerId> customerIds,
            CancellationToken cancellationToken = default)
        {
            var startDate = DateOnly.MinValue;
            var endDate = DateOnly.MaxValue;
            if(datePeriod != null && datePeriod != DatePeriod.Empty())
            {
                startDate = datePeriod.StartDate;
                endDate = datePeriod.EndDate;
            }

            var filteringCriteria = string.Empty;
            
            if(productNames != null && !productNames.IsEmpty())
            {
                filteringCriteria += $"AND data->'{nameof(FuelTransaction.ProductName)}'->>'Value' IN({string.Join(",", productNames.Select(c=> "'" + c.Value + "'"))})";
            }

            if(customerIds != null && !customerIds.IsEmpty())
            {
                if(!filteringCriteria.IsEmpty())
                {
                    filteringCriteria += "\n";
                }
                filteringCriteria += $"AND (data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' IN({string.Join(",", customerIds.Select(c=> "'" + c.Value + "'"))}) OR data->'{nameof(FuelTransaction.AccountCustomerId)}'->>'Value' IN({string.Join(",", customerIds.Select(c => "'" + c.Value + "'"))}))";
            }

            var sql = $@"
                SELECT
                    (data->'{nameof(FuelTransaction.FuelTransactionDate)}'->>'Value') as FuelTransactionDate,
                    (data->'{nameof(FuelTransaction.ProductName)}'->>'Value') as ProductName,
                    (data->'{nameof(FuelTransaction.ProductDescription)}'->>'Value') as ProductDescription,
                    SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric) as Quantity
                FROM {schemaName}.mt_doc_fueltransaction
                WHERE
                    fuel_transaction_time_stamp::timestamp >= :startDate::date AND fuel_transaction_time_stamp::timestamp <= :endDate::date
                    {filteringCriteria}
                GROUP BY
                    data->'{nameof(FuelTransaction.ProductName)}'->>'Value',
                    data->'{nameof(FuelTransaction.ProductDescription)}'->>'Value',
                    data->'{nameof(FuelTransaction.FuelTransactionDate)}'->>'Value'
                ORDER BY data->'{nameof(FuelTransaction.FuelTransactionDate)}'->>'Value' ASC";

            var parameters = new
            {
                startDate = startDate,
                endDate = endDate
            };

            var rowsSimple = await documentSession.Connection!.QueryAsync<SimpleGroupedFuelTransaction>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));

            var rows = rowsSimple.Select(c => c.AsGroupedFuelTransaction()).ToList();

            return rows.ToList();
        }
    }
}