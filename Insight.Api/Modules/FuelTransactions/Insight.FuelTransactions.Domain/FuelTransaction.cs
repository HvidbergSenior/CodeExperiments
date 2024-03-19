using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Serialization;
using System.Globalization;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransaction : Entity
    {
        public FuelTransactionCustomerId CustomerId { get; private set; } = FuelTransactionCustomerId.Empty();
        public CustomerName CustomerName { get; private set; } = CustomerName.Empty();
        public CustomerType CustomerType { get; private set; } = CustomerType.Empty();
        public CustomerSegment CustomerSegment { get; private set; } = CustomerSegment.Empty();
        public FuelTransactionId FuelTransactionId { get; private set; }
        public FuelTransactionPosSystem FuelTransactionPosSystem { get; private set; }
        public StationNumber StationNumber { get; private set; }
        public StationName StationName { get; private set; }
        public FuelTransactionDate FuelTransactionDate { get; private set; }
        public FuelTransactionTime FuelTransactionTime { get; private set; }
        public DateTimeOffset FuelTransactionTimeStamp => ParseTimeStamp();
        public ProductNumber ProductNumber { get; private set; }
        public ProductName ProductName { get; private set; }
        public ProductDescription ProductDescription { get; private set; } = ProductDescription.Empty();
        public Quantity Quantity { get; private set; }
        public Odometer Odometer { get; private set; }
        public CardTypeId CardTypeId { get; private set; } = CardTypeId.Empty();
        public CardAcceptanceId CardAcceptanceId { get; private set; } = CardAcceptanceId.Empty();
        public DriverCardNumber DriverCardNumber { get; private set; }
        public VehicleCardNumber VehicleCardNumber { get; private set; }
        public CustomerNumber CustomerNumber { get; private set; }
        public Pump Pump { get; private set; } = Pump.Empty();
        public SourceETag SourceETag { get; private set; }
        public FuelTransactionCountry Country { get; private set; }
        public SourceSystemPropertyBag SourceSystemPropertyBag { get; private set; } = SourceSystemPropertyBag.Empty();
        public SourceSystemId SourceSystemId { get; private set; } = SourceSystemId.Empty();
        public Location Location { get; private set; } = Location.Empty();
        public AccountNumber AccountNumber { get; private set; } = AccountNumber.Empty();
        public AccountName AccountName { get; private set; } = AccountName.Empty();
        public AccountCustomerId AccountCustomerId { get; private set; } = AccountCustomerId.Empty();
        public string LocationId => $"{Country.Value}:{StationNumber.Value}:{StationName.Value}";
        public string ItemHash => HashHelpers.GetHashCode(FuelTransactionPosSystem, SourceSystemId.Value);

        public DraftAllocationId DraftAllocationId { get; private set; } = DraftAllocationId.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();

        public ShipToLocation ShipToLocation { get; private set; } = ShipToLocation.Empty();
        public Driver Driver { get; private set; } = Driver.Empty();

        private FuelTransaction()
        {
            FuelTransactionId = FuelTransactionId.Empty();
            Id = FuelTransactionId.Value;
            FuelTransactionPosSystem = FuelTransactionPosSystem.Tokheim;
            StationNumber = StationNumber.Empty();
            StationName = StationName.Empty();
            FuelTransactionDate = FuelTransactionDate.Empty();
            FuelTransactionTime = FuelTransactionTime.Empty();
            ProductNumber = ProductNumber.Empty();
            ProductName = ProductName.Empty();
            Quantity = Quantity.Empty();
            Odometer = Odometer.Empty();
            DriverCardNumber = DriverCardNumber.Empty();
            VehicleCardNumber = VehicleCardNumber.Empty();
            CustomerNumber = CustomerNumber.Empty();
            CustomerName = CustomerName.Empty();
            SourceETag = SourceETag.Empty();
            Country = FuelTransactionCountry.Empty();
            CompanyName = CompanyName.Empty();
        }

        private FuelTransaction(
            FuelTransactionId fuelTransactionId,
            FuelTransactionPosSystem fuelTransactionStatePosSystem,
            StationNumber stationNumber,
            StationName stationName,
            FuelTransactionDate fuelTransactionDate,
            FuelTransactionTime fuelTransactionTime,
            ProductNumber productNumber,
            ProductName productName,
            Quantity quantity,
            Odometer odometer,
            DriverCardNumber driverCardNumber,
            VehicleCardNumber vehicleCardNumber,
            CustomerNumber customerNumber,
            CustomerName customerName,
            SourceETag sourceETag,
            FuelTransactionCountry country,
            SourceSystemPropertyBag sourceSystemPropertyBag,
            SourceSystemId sourceSystemId,
            Location location,
            CustomerType customerType,
            CustomerSegment customerSegment,
            CompanyName companyName,
            AccountNumber accountNumber,
            AccountName accountName,
            AccountCustomerId accountCustomerId,
            ProductDescription productDescription,
            ShipToLocation shipToLocation,
            Driver driver
        )
        {
            FuelTransactionId = fuelTransactionId;
            Id = FuelTransactionId.Value;
            FuelTransactionPosSystem = fuelTransactionStatePosSystem;
            StationNumber = stationNumber;
            StationName = stationName;
            FuelTransactionDate = fuelTransactionDate;
            FuelTransactionTime = fuelTransactionTime;
            ProductNumber = productNumber;
            ProductName = productName;
            Quantity = quantity;
            Odometer = odometer;
            DriverCardNumber = driverCardNumber;
            VehicleCardNumber = vehicleCardNumber;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            SourceETag = sourceETag;
            Country = country;
            SourceSystemPropertyBag = sourceSystemPropertyBag;
            SourceSystemId = sourceSystemId;
            Location = location;
            CustomerType = customerType;
            CustomerSegment = customerSegment;
            CompanyName = companyName;
            AccountNumber = accountNumber;
            AccountName = accountName;
            AccountCustomerId = accountCustomerId;
            ProductDescription = productDescription;
            ShipToLocation = shipToLocation;
            Driver = driver;
        }

        public static FuelTransaction Create(
            FuelTransactionId fuelTransactionId,
            FuelTransactionPosSystem fuelTransactionStatePosSystem,
            StationNumber stationNumber,
            StationName stationName,
            FuelTransactionDate fuelTransactionDate,
            FuelTransactionTime fuelTransactionTime,
            ProductNumber productNumber,
            ProductName productName,
            Quantity quantity,
            Odometer odometer,
            DriverCardNumber driverCardNumber,
            VehicleCardNumber vehicleCardNumber,
            CustomerNumber customerNumber,
            CustomerName customerName,
            SourceETag sourceETag,
            FuelTransactionCountry country,
            SourceSystemPropertyBag sourceSystemPropertyBag,
            SourceSystemId sourceSystemId,
            Location location,
            CustomerType customerType,
            CustomerSegment customerSegment,
            CompanyName companyName,
            AccountNumber accountNumber,
            AccountName accountName,
            AccountCustomerId accountCustomerId, 
            ProductDescription productDescription,
            ShipToLocation shipToLocation,
            Driver driver)
        {
            return new FuelTransaction(
                fuelTransactionId,
                fuelTransactionStatePosSystem,
                stationNumber,
                stationName,
                fuelTransactionDate,
                fuelTransactionTime,
                productNumber,
                productName,
                quantity,
                odometer,
                driverCardNumber,
                vehicleCardNumber,
                customerNumber,
                customerName,
                sourceETag,
                country,
                sourceSystemPropertyBag,
                sourceSystemId,
                location,
                customerType,
                customerSegment,
                companyName,
                accountNumber, 
                accountName,
                accountCustomerId,
                productDescription,
                shipToLocation,
                driver);
        }

        public void SetCustomerId(FuelTransactionCustomerId customerId)
        {
            CustomerId = customerId;
        }

        public static FuelTransaction Empty()
        {
            return new FuelTransaction();
        }

        private DateTimeOffset ParseTimeStamp()
        {
            var tz = GetTimeZoneInfoByCountry(Country.Value);
            var ci = GetCultureInfoByCountry(Country.Value);

            var date = DateOnly.ParseExact(FuelTransactionDate.Value, SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, ci);
            var time = TimeOnly.Parse(FuelTransactionTime.Value, ci);

            var offSet = tz.GetUtcOffset(date.ToDateTime(time));

            return new DateTimeOffset(date.ToDateTime(time), offSet).ToUniversalTime();
        }


        private static CultureInfo GetCultureInfoByCountry(string country)
        {
            switch (country.ToLowerInvariant())
            {
                case "se":
                    return new CultureInfo("sv-SE");
                case "no":
                    return new CultureInfo("nb-NO");
                case "fi":
                    return new CultureInfo("fi-FI");
                case "dk":
                default:
                    return new CultureInfo("da-DK");
            }
        }

        private static TimeZoneInfo GetTimeZoneInfoByCountry(string country)
        {
            switch (country.ToLowerInvariant())
            {
                case "se":
                    return TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                case "no":
                    return TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                case "fi":
                    return TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
                case "dk":
                default:
                    return TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

            }
        }

        public void SetFuelTransactionDate(FuelTransactionDate fuelTransactionDate)
        {
            FuelTransactionDate = fuelTransactionDate;
        }
        public void SetFuelTransactionTime(FuelTransactionTime fuelTransactionTime)
        {
            FuelTransactionTime = fuelTransactionTime;
        }

        public void SetDraftAllocationId(DraftAllocationId draftAllocationId)
        {
            DraftAllocationId = draftAllocationId;
        }
    }
}