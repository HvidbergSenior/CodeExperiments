using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Allocations;
using Marten;
using PaginationParameters = Insight.FuelTransactions.Domain.PaginationParameters;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Infrastructure
{
    public class FuelTransactionsInMemoryRepository : InMemoryRepository<FuelTransaction>, IFuelTransactionsRepository, IFuelTransactionsAllocationRepository
    {
        public async Task<IEnumerable<FuelTransaction>> GetFuelTransactionsBetweenDates(FuelTransactionsBetweenDates fuelTransactionsBetweenDates,
            CancellationToken cancellationToken = default)
        {
            var fromDateTimeOffSet = new DateTimeOffset(fuelTransactionsBetweenDates.FromDate.Year, fuelTransactionsBetweenDates.FromDate.Month, fuelTransactionsBetweenDates.FromDate.Day, 0, 0, 0, TimeSpan.Zero);
            var toDateTimeOffset = new DateTimeOffset(fuelTransactionsBetweenDates.ToDate.Year, fuelTransactionsBetweenDates.ToDate.Month, fuelTransactionsBetweenDates.ToDate.Day, 0, 0, 0, TimeSpan.Zero);
            var query = Query().Where(query => fromDateTimeOffSet <= query.FuelTransactionTimeStamp &&
                                               toDateTimeOffset >= query.FuelTransactionTimeStamp);
            return await IQueryableExtension.ToMartenListAsync(query, cancellationToken);
        }

        public Task<(IEnumerable<FuelTransaction> Transactions, int TotalCount)> FuelTransactionWithCount()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductName>> GetProductNamesForCustomerId(FuelTransactionCustomerId customerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerName>> GetCustomerNamesForCustomerId(FuelTransactionCustomerId customerId)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<FuelTransaction> Transactions, int TotalCount)> GetFuelTransactionsRefinedAsync(FuelTransactionsBetweenDates fuelTransactionsBetweenDates,
            IEnumerable<ProductName> productNames,
            IEnumerable<FuelTransactionCustomerId> customerIds,
            PaginationParameters paginationParameters,
            SortingParameters sortingParameters,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Query().AnyAsync(cancellationToken);
        }

        public async Task ClearAllocationDraftIdAsync(DraftAllocationId draftAllocationId, CancellationToken cancellationToken)
        {
            foreach (var entity in Query().Where(c => c.DraftAllocationId.Value == draftAllocationId.Value))
            {
                entity.SetDraftAllocationId(DraftAllocationId.Empty());
                await Update(entity, cancellationToken);
            }
        }

        public async Task ClearAllocationDraftIdFromBatchAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            foreach (var entity in Query().Where(c => c.DraftAllocationId.Value == draftAllocationId.Value && c.CustomerId.Value == fuelTransactionCustomerId.Value && c.ProductNumber.Value == productNumber.Value && c.LocationId == locationId.Value))
            {
                entity.SetDraftAllocationId(DraftAllocationId.Empty());
                await Update(entity, cancellationToken);
            }
        }

        public async Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            var transactions = new List<FuelTransaction>();
            foreach (var entity in Query().Where(c => c.CustomerId.Value == customerId.Value
                && c.ProductNumber.Value == productNumber.Value
                && c.LocationId == locationId.Value
                && c.FuelTransactionTimeStamp >= startDate.ToDateTime(TimeOnly.MinValue)
                && c.FuelTransactionTimeStamp <= endDate.ToDateTime(TimeOnly.MaxValue)).ToList())
            {
                entity.SetDraftAllocationId(draftAllocationId);
                transactions.Add(entity);
                await Update(entity, cancellationToken);
            }
            return new DraftAllocationRowResponse()
            {
                FuelTransactionIds = transactions.Select(c => c.FuelTransactionId).ToArray(),
                Quantity = Quantity.Create(transactions.Sum(c => c.Quantity.Value))
            };
        }

        public async Task<IEnumerable<FuelTransactionId>> GetFuelTransactionIdsByDraftAllocationIdAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            var result = Query().Where(c => c.DraftAllocationId.Value == draftAllocationId.Value && c.CustomerId.Value == customerId.Value && c.ProductNumber.Value == productNumber.Value && c.LocationId == locationId.Value && c.FuelTransactionTimeStamp >= startDate.ToDateTime(TimeOnly.MinValue) && c.FuelTransactionTimeStamp <= endDate.ToDateTime(TimeOnly.MaxValue)).Select(c => c.FuelTransactionId);
            return await IQueryableExtension.ToMartenListAsync(result, cancellationToken);
        }

        public async Task<IEnumerable<MissingAllocationRowResponse>> GetMissingAllocationForPeriodAndFiltersAsync(DateOnly startDate, DateOnly endDate, string product, string customer, string company, CancellationToken cancellationToken)
        {
            //Filtering (product, customer, company) is not implemented in InMemory 
            var alreadyAllocatedResult = Query().Where(c => c.FuelTransactionTimeStamp >= startDate.ToDateTime(TimeOnly.MinValue) && c.FuelTransactionTimeStamp <= endDate.ToDateTime(TimeOnly.MaxValue) &&
            c.DraftAllocationId.Value != Guid.Empty).GroupBy(c => new { c.CustomerId, c.ProductNumber, c.LocationId, c.Country, c.Location, c.ProductName });

            var notAllocated = Query().Where(c => c.FuelTransactionTimeStamp >= startDate.ToDateTime(TimeOnly.MinValue) && c.FuelTransactionTimeStamp <= endDate.ToDateTime(TimeOnly.MaxValue) &&
            c.DraftAllocationId.Value == Guid.Empty).GroupBy(c => new { c.CustomerId, c.ProductNumber, c.LocationId, c.Country, c.Location, c.ProductName, c.CompanyName });

            var result = notAllocated.GroupJoin(alreadyAllocatedResult, na => na.Key.ProductName, a => a.Key.ProductName, (x, y) => new { NotAllocated = x, Allocated = y }).
                SelectMany(c => c.Allocated.DefaultIfEmpty(), (x, y) => new { NotAllocated = x.NotAllocated, Allocated = y }).
                Select(c => MissingAllocationRowResponse.Create(FuelTransactionCustomerId.Create(c.NotAllocated.Key.CustomerId.Value),
                ProductNumber.Create(c.NotAllocated.Key.ProductNumber.Value),
                LocationId.Create(c.NotAllocated.Key.LocationId),
                Quantity.Create(c.NotAllocated.Sum(c => c.Quantity.Value)),
                FuelTransactionCountry.Create(c.NotAllocated.Key.Country.Value),
                Quantity.Create(c.Allocated == null ? 0 : c.Allocated.Sum(o=> o.Quantity.Value)), // this is probably wrong
                Location.Create(c.NotAllocated.Key.Location.Value), 
                ProductName.Create(c.NotAllocated.Key.ProductName.Value),
                CompanyName.Create(c.NotAllocated.Key.CompanyName.Value)));
            
            return await IQueryableExtension.ToMartenListAsync(result, cancellationToken);
        }

        public Task<IEnumerable<GroupedFuelTransaction>> GetGroupedTransactions(
            DatePeriod datePeriod,
            IEnumerable<ProductName> productNames,
            IEnumerable<CustomerId> customerIds,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            foreach (var entity in Query().Where(c => c.DraftAllocationId.Value == draftAllocationId.Value && c.CustomerId.Value == fuelTransactionCustomerId.Value && c.ProductNumber.Value == productNumber.Value && c.LocationId == locationId.Value && c.FuelTransactionTimeStamp >= startDate.ToDateTime(TimeOnly.MinValue) && c.FuelTransactionTimeStamp <= endDate.ToDateTime(TimeOnly.MaxValue)))
            {
                entity.SetDraftAllocationId(DraftAllocationId.Empty());
                await Update(entity, cancellationToken);
            }
        }

        public Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, FuelTransactionId[] fuelTransactionIds, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsPartiallyAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, Quantity maxQuantity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetExistingHashesAsync(string[] itemHashes, CancellationToken cancellationToken)
        {
            var result = Query().Where(c => c.ItemHash.In(itemHashes)).Select(c => c.ItemHash);
            return await IQueryableExtension.ToMartenListAsync(result, cancellationToken);
        }
    }
}
