using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.Stock
{
    public sealed class StockAllocations : ValueObject
    {
        public Dictionary<Guid, decimal> Value { get; private set; } = new Dictionary<Guid, decimal>();
        public decimal TotalAllocatedVolume => Value.Sum(x => x.Value);

        private StockAllocations()
        {
            // Intentionally left empty
        }

        private StockAllocations(Dictionary<Guid, decimal> value)
        {
            Value = value;
        }

        public static StockAllocations Create(Dictionary<Guid, decimal> value)
        {
            return new StockAllocations(value);
        }

        public static StockAllocations Empty()
        {
            return new StockAllocations();
        }
    }
}
