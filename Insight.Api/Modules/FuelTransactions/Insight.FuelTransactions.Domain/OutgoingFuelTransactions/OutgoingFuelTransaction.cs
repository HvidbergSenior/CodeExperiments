using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.OutgoingFuelTransactions
{
    public sealed class OutgoingFuelTransaction : ValueObject
    {
        public StationNumber StationNumber { get; private set; } = StationNumber.Empty();
        public StationName StationName { get; private set; } = StationName.Empty();
        public ProductNumber ProductNumber { get; private set; } = ProductNumber.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public Quantity Quantity { get; private set; } = Quantity.Empty();
        public ItemCount ItemCount { get; private set; } = ItemCount.Empty();
        public CustomerNumber CustomerNumber { get; private set; } = CustomerNumber.Empty();
        public CustomerName CustomerName { get; private set; } = CustomerName.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();        
        public FuelTransactionCountry Country { get; private set; } = FuelTransactionCountry.Empty();
        public Location Location { get; private set; } = Location.Empty();
        public ShipToLocation ShipToLocation { get; private set; } = ShipToLocation.Empty();
        public FuelTransactionCustomerId CustomerId { get; private set; } = FuelTransactionCustomerId.Empty();
        public CustomerType CustomerType { get; private set; } = CustomerType.Empty();
        public CustomerSegment CustomerSegment { get; private set; } = CustomerSegment.Empty();
        public LocationId LocationId { get; private set; } = LocationId.Empty();
        public Quantity AllocatedQuantity { get; private set; } = Quantity.Empty();
        public Percentage AlreadyAllocatedPercentage { get; private set; } = Percentage.Empty();
        public Quantity MissingAllocationQuantity { get; private set; } = Quantity.Empty();
        public Quantity TotalQuantity { get; private set; } = Quantity.Empty();
        private OutgoingFuelTransaction(FuelTransactionCustomerId customerId, CompanyName companyName,  StationNumber stationNumber, StationName stationName, ProductNumber productNumber, ProductName productName, Quantity quantity, CustomerNumber customerNumber, CustomerName customerName, FuelTransactionCountry fuelTransactionCountry, ItemCount itemCount, Location location, CustomerType customerType, CustomerSegment customerSegment, LocationId locationId, Quantity allocatedQuantity, Percentage alreadyAllocatedPercentage, Quantity missingAllocationQuantity, Quantity totalQuantity, ShipToLocation shipToLocation)
        {
            CustomerId = customerId;
            CompanyName = companyName;
            StationNumber = stationNumber;
            StationName = stationName;
            ProductNumber = productNumber;
            ProductName = productName;
            Quantity = quantity;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            Country = fuelTransactionCountry;
            ItemCount = itemCount;
            Location = location;
            CustomerType = customerType;
            CustomerSegment = customerSegment;
            LocationId = locationId;
            AllocatedQuantity = allocatedQuantity;
            AlreadyAllocatedPercentage = alreadyAllocatedPercentage;
            MissingAllocationQuantity = missingAllocationQuantity;
            TotalQuantity = totalQuantity;
            ShipToLocation = shipToLocation;
        }

        private OutgoingFuelTransaction()
        {
            //Left empty for serialization purposes
        }

        public static OutgoingFuelTransaction Empty()
        {
            return new OutgoingFuelTransaction();
        }

        public static OutgoingFuelTransaction Create(FuelTransactionCustomerId fuelTransactionCustomerId, CompanyName companyName, StationNumber stationNumber, StationName stationName, ProductNumber productNumber, ProductName productName, Quantity quantity, CustomerNumber customerNumber, CustomerName customerName, FuelTransactionCountry fuelTransactionCountry, ItemCount itemCount, Location location, CustomerType customerType, CustomerSegment customerSegment, LocationId locationId, Quantity allocatedQuantity, Percentage alreadyAllocatedPercentage, Quantity missingAllocationQuantity, Quantity totalQuantity, ShipToLocation shipToLocation)
        {
            return new OutgoingFuelTransaction(fuelTransactionCustomerId, companyName, stationNumber, stationName, productNumber, productName, quantity, customerNumber, customerName, fuelTransactionCountry, itemCount, location, customerType, customerSegment, locationId, allocatedQuantity, alreadyAllocatedPercentage, missingAllocationQuantity, totalQuantity, shipToLocation);
        }
    }
}
