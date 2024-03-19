using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class FuelTransactionsBatch : ValueObject
    {
        public FuelTransactionCustomerId CustomerId { get; set; } = FuelTransactionCustomerId.Empty();
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public ProductNumber ProductNumber { get; set; } = ProductNumber.Empty();
        public FuelTransactionCountry Country { get; set; } = FuelTransactionCountry.Empty();
        public StationName StationName { get; set; } = StationName.Empty();
        public ProductName ProductName { get; set; } = ProductName.Empty(); 
        public LocationId LocationId { get; set; } = LocationId.Empty();

        private FuelTransactionsBatch(FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, FuelTransactionCountry country, StationName stationName, ProductName productName, LocationId locationId)
        {
            CustomerId = customerId;
            StartDate = startDate;
            EndDate = endDate;
            ProductNumber = productNumber;
            Country = country;
            StationName = stationName;
            ProductName = productName;
            LocationId = locationId;
        }

        public static FuelTransactionsBatch Create(FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, FuelTransactionCountry country, StationName stationName, ProductName productName, LocationId locationId)
        {
            return new FuelTransactionsBatch(customerId, startDate, endDate, productNumber, country, stationName, productName, locationId);
        }

        public static FuelTransactionsBatch Empty()
        {
            return new FuelTransactionsBatch(FuelTransactionCustomerId.Empty(), DateOnly.MinValue, DateOnly.MinValue, ProductNumber.Empty(), FuelTransactionCountry.Empty(), StationName.Empty(), ProductName.Empty(),  LocationId.Empty());
        }

    }
}
