namespace Insight.FuelTransactions.Domain
{
    public sealed class SimpleGroupedFuelTransaction
    {
        public string FuelTransactionDate { get; private set; } = string.Empty;
        public string ProductName { get; private set; } = string.Empty;
        public string ProductDescription { get; private set; } = string.Empty;
        public decimal Quantity { get; private set; }

        public GroupedFuelTransaction AsGroupedFuelTransaction()
        {
            return GroupedFuelTransaction.Create(
                Domain.FuelTransactionDate.Create(FuelTransactionDate),
                BuildingBlocks.Domain.ProductName.Create(ProductName),
                Domain.Quantity.Create(Quantity),
                Domain.ProductDescription.Create(ProductDescription));
        }
    }
}
