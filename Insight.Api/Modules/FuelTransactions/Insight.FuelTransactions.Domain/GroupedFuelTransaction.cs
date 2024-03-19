using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class GroupedFuelTransaction : ValueObject
    {
        public FuelTransactionDate FuelTransactionDate { get; private set; }
        public ProductName ProductName { get; private set; }
        public ProductDescription ProductDescription { get; private set; } = ProductDescription.Empty();
        public Quantity Quantity { get; private set; }

        private GroupedFuelTransaction()
        {
            FuelTransactionDate = FuelTransactionDate.Empty();
            ProductName = ProductName.Empty();
            Quantity = Quantity.Empty();
            ProductDescription = ProductDescription.Empty();
        }

        private GroupedFuelTransaction(
            FuelTransactionDate fuelTransactionDate,
            ProductName productName,
            Quantity quantity,
            ProductDescription productDescription)
        {
            FuelTransactionDate = fuelTransactionDate;
            ProductName = productName;
            Quantity = quantity;
            ProductDescription = productDescription;
        }

        public static GroupedFuelTransaction Create(
            FuelTransactionDate fuelTransactionDate,
            ProductName productName,
            Quantity quantity,
            ProductDescription productDescription)
        {
            return new GroupedFuelTransaction(fuelTransactionDate, productName, quantity, productDescription);
        }
    }
}