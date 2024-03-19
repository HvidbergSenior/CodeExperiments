using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain.Allocations
{
    public sealed class MissingAllocationRowResponse : ValueObject
    {
        public FuelTransactionCustomerId FuelTransactionCustomerId { get; private set; } = FuelTransactionCustomerId.Empty();
        public ProductNumber ProductNumber { get; private set; } = ProductNumber.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public FuelTransactionCountry FuelTransactionCountry { get; private set; } = FuelTransactionCountry.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();
        public LocationId LocationId { get; private set; } = LocationId.Empty();
        public Location Location { get; private set; } = Location.Empty();
        public Quantity Quantity { get; private set; } = Quantity.Empty();
        public Quantity AllocatedQuantity { get; private set; } = Quantity.Empty();
        private MissingAllocationRowResponse(FuelTransactionCustomerId fuelTransactionCustomerId, ProductNumber productNumber, LocationId locationId, Quantity quantity, FuelTransactionCountry fuelTransactionCountry, Quantity allocatedQuantity, Location location, ProductName productName, CompanyName companyName)
        {
            FuelTransactionCustomerId = fuelTransactionCustomerId;
            ProductNumber = productNumber;            
            LocationId = locationId;
            Quantity = quantity;
            FuelTransactionCountry = fuelTransactionCountry;
            AllocatedQuantity = allocatedQuantity;
            Location = location;
            ProductName = productName;
            CompanyName = companyName;
        }

        public static MissingAllocationRowResponse Create(FuelTransactionCustomerId fuelTransactionCustomerId, ProductNumber productNumber, LocationId locationId, Quantity quantity, FuelTransactionCountry fuelTransactionCountry, Quantity allocatedQuantity, Location location, ProductName productName, CompanyName companyName)
        {
            return new MissingAllocationRowResponse(fuelTransactionCustomerId, productNumber, locationId, quantity, fuelTransactionCountry, allocatedQuantity, location, productName, companyName);
        }
    }
}
