using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain.SustainabilityReports
{
    public sealed class OutgoingFuelTransaction : ValueObject
    {
        public ProductName ProductName { get; set; } = ProductName.Empty();
        public Quantity Quantity { get; set; } = Quantity.Empty();
        public CustomerName CustomerName { get; set; } = CustomerName.Empty();
        public ItemCount ItemCount { get; set; } = ItemCount.Empty();
        public CustomerId CustomerId { get; set; } = CustomerId.Empty();
        public FuelTransactionMonth FuelTransactionMonth { get; set; } = FuelTransactionMonth.Empty();
        public FuelTransactionYear FuelTransactionYear { get; set; } = FuelTransactionYear.Empty();

        private OutgoingFuelTransaction(ProductName productName, Quantity quantity, CustomerId customerId, CustomerName customerName, ItemCount itemCount, FuelTransactionMonth fuelTransactionMonth, FuelTransactionYear fuelTransactionYear)
        {
            ProductName = productName;
            Quantity = quantity;
            CustomerId = customerId;
            CustomerName = customerName;
            ItemCount = itemCount;
            FuelTransactionMonth = fuelTransactionMonth;
            FuelTransactionYear = fuelTransactionYear;
        }

        private OutgoingFuelTransaction()
        {
            //Left empty for serialization purposes
        }

        public static OutgoingFuelTransaction Empty()
        {
            return new OutgoingFuelTransaction();
        }

        public static OutgoingFuelTransaction Create(ProductName productName, Quantity quantity, CustomerId customerNumber, CustomerName customerName, ItemCount itemCount, FuelTransactionMonth fuelTransactionMonth, FuelTransactionYear fuelTransactionYear)
        {
            return new OutgoingFuelTransaction(productName, quantity, customerNumber, customerName, itemCount, fuelTransactionMonth, fuelTransactionYear);
        }
    }
}
