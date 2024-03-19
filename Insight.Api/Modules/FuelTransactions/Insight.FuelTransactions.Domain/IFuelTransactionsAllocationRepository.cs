using Insight.FuelTransactions.Domain.Allocations;

namespace Insight.FuelTransactions.Domain
{
    public interface IFuelTransactionsAllocationRepository
    {
        public Task ClearAllocationDraftIdAsync(DraftAllocationId draftAllocationId, CancellationToken cancellationToken);
        public Task ClearAllocationDraftIdFromBatchAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken);
        public Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken);
        public Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsPartiallyAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, Quantity maxQuantity, CancellationToken cancellationToken);
        public Task<IEnumerable<FuelTransactionId>> GetFuelTransactionIdsByDraftAllocationIdAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken);
        public Task<IEnumerable<MissingAllocationRowResponse>> GetMissingAllocationForPeriodAndFiltersAsync(DateOnly startDate, DateOnly endDate, string product, string company, string customer, CancellationToken cancellationToken);
        public Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken);
        public Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, FuelTransactionId[] fuelTransactionIds, CancellationToken cancellationToken);
    }
}
