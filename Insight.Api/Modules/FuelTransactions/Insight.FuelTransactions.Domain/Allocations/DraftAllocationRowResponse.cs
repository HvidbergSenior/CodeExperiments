namespace Insight.FuelTransactions.Domain.Allocations
{
    public class DraftAllocationRowResponse
    {
        public FuelTransactionId[] FuelTransactionIds { get; set; } = Array.Empty<FuelTransactionId>();
        public Quantity Quantity { get; set; } = Quantity.Empty();
    }
}
