using AutoFixture;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;
using System.Globalization;
using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Tests
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }
        internal static FuelTransactionsBetweenDates FuelTransactionBetweenDatesRandomDates()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;

            return Domain.FuelTransactionsBetweenDates.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
        }
        internal static FuelTransactionsBetweenDates FuelTransactionBetweenDates10YearsAgoAndNow()
        {
            var fromDate = DateTime.Now.AddYears(-10);
            var toDate = DateTime.Now;

            return Domain.FuelTransactionsBetweenDates.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(toDate));
        }
        internal static FuelTransaction FuelTransaction()
        {
            var fuelTransactionId = FuelTransactionId();
            const FuelTransactionPosSystem fuelTransactionPosSystem = FuelTransactionPosSystem.Dialog;
            var stationNumber = StationNumber();
            var stationName = StationName();
            var customerName = CustomerName();
            var fuelTransactionDate = FuelTransactionDate();
            var fuelTransactionTime = FuelTransactionTime();
            var productNumber = ProductNumber();
            var productName = ProductName();
            var quantity = Quantity();
            var odometer = Odometer();
            var cardTypeId = CardTypeId();
            var cardAcceptanceId = CardAcceptanceId();
            var driverCardNumber = DriverCardNumber();
            var vehicleCardNumber = VehicleCardNumber();
            var customerNumber = CustomerNumber();
            var pump = Pump();
            var sourceETag = SourceETag();
            var country = Country();
            var sourceSystemPropertyBag = SourceSystemPropertyBag();
            var sourceSystemId = SourceSystemId();
            var location = Location();
            var customerType = CustomerType();
            var customerSegment = CustomerSegment();
            var companyName = CompanyName();
            var accountNumber = AccountNumber();
            var accountName = AccountName();
            var accountCustomerID = AccountCustomerId();
            var productDescription = ProductDescription();
            var shipToLocation = ShipToLocation();
            var driver = Driver();

            return Domain.FuelTransaction.Create(
                fuelTransactionId,
                fuelTransactionPosSystem,
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
                accountCustomerID,
                productDescription,
                shipToLocation,
                driver);
        }

        internal static FuelTransaction FuelTransaction(DateTime timeStamp, FuelTransactionCustomerId fuelTransactionCustomerId, CustomerNumber customerNumber, CustomerName customerName, CustomerType customerType, CustomerSegment customerSegment, ProductNumber productNumber, ProductName productName, FuelTransactionCountry fuelTransactionCountry, StationName stationName, StationNumber stationNumber, Quantity quantity, Location location, CompanyName companyName)
        {

            var date = timeStamp.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var time = timeStamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

            var ft = Domain.FuelTransaction.Create(FuelTransactionId(),
                                                 FuelTransactionPosSystem.Dialog,
                                                 stationNumber,
                                                 stationName,
                                                 Domain.FuelTransactionDate.Create(date),
                                                 Domain.FuelTransactionTime.Create(time),
                                                 productNumber,
                                                 productName,
                                                 quantity,
                                                 Odometer(),
                                                 DriverCardNumber(),
                                                 VehicleCardNumber(),
                                                 customerNumber,
                                                 customerName,
                                                 SourceETag(),
                                                 fuelTransactionCountry,
                                                 SourceSystemPropertyBag(),
                                                 SourceSystemId(),
                                                 location,
                                                 customerType,
                                                 customerSegment,
                                                 companyName,
                                                 AccountNumber(),
                                                 AccountName(),
                                                 AccountCustomerId(),
                                                 ProductDescription(),
                                                 ShipToLocation(),
                                                 Driver());
            
            ft.SetCustomerId(fuelTransactionCustomerId);
            
            return ft;
        }

        private static ProductDescription ProductDescription()
        {
            return Domain.ProductDescription.Create(Instance<string>());
        }

        private static Driver Driver()
        {
            return Domain.Driver.Create(Instance<string>());
        }

        private static ShipToLocation ShipToLocation()
        {
            return Domain.ShipToLocation.Create(Instance<string>());
        }

        private static AccountCustomerId AccountCustomerId()
        {
            return Domain.AccountCustomerId.Create(Instance<Guid>());
        }

        private static AccountName AccountName()
        {
            return Domain.AccountName.Create(Instance<string>());
        }

        private static AccountNumber AccountNumber()
        {
            return Domain.AccountNumber.Create(Instance<string>());
        }

        private static SourceSystemPropertyBag SourceSystemPropertyBag()
        {
            return Domain.SourceSystemPropertyBag.Create(Instance<string>());
        }
        private static SourceSystemId SourceSystemId()
        {
            return Domain.SourceSystemId.Create(Instance<Guid>());
        }        
        private static CustomerType CustomerType()
        {
            return Domain.CustomerType.Create(Instance<string>());
        }
        private static CustomerSegment CustomerSegment()
        {
            return Domain.CustomerSegment.Create(Instance<string>());
        }
       
        private static FuelTransactionId FuelTransactionId()
        {
            return Domain.FuelTransactionId.Create(Instance<Guid>());
        }
        public static OutgoingFuelTransaction OutgoingFuelTransaction()
        {
            return Domain.OutgoingFuelTransactions.OutgoingFuelTransaction
                .Create(Any.FuelTransactionCustomerId(), Any.CompanyName(), Any.StationNumber(), Any.StationName(), Any.ProductNumber(), Any.ProductName(), Any.Quantity(),
                    Any.CustomerNumber(), Any.CustomerName(), Any.Country(), Any.ItemCount(), Any.Location(), Any.CustomerType(), Any.CustomerSegment(), Any.LocationId(), Any.Quantity(), Any.Percentage(), Any.Quantity(), Any.Quantity(), Any.ShipToLocation());
        }
        public static LocationId LocationId()
        {
            return Domain.LocationId.Create(Instance<string>());
        }

        public static Percentage Percentage()
        {
            return Domain.Percentage.Create(Instance<decimal>());
        }

        public static FuelTransactionCustomerId FuelTransactionCustomerId()
        {
            return Domain.FuelTransactionCustomerId.Create(Instance<Guid>());
        }
        public static CompanyName CompanyName()
        {
            return Domain.CompanyName.Create(Instance<string>());
        }
        public static StationNumber StationNumber()
        {
            return Domain.StationNumber.Create(Instance<string>());
        }

        public static Location Location()
        {
            return Domain.Location.Create(Instance<string>());
        }

        private static string GetRandomValidProductName()
        {
            var validProductNames = new List<string> { "HVO DIESEL", "B100" };
            return validProductNames.OrderBy(c => Guid.NewGuid()).First();
        }

        public static ProductName ProductName()
        {
            return BuildingBlocks.Domain.ProductName.Create(GetRandomValidProductName());
        }

        public static CustomerName CustomerName()
        {
            return BuildingBlocks.Domain.CustomerName.Create(Instance<string>());
        }

        public static StationName StationName()
        {
            return Domain.StationName.Create(Instance<string>());
        }

        private static FuelTransactionTime FuelTransactionTime()
        {
            return Domain.FuelTransactionTime.Create(Instance<string>());
        }

        private static FuelTransactionDate FuelTransactionDate()
        {
            return Domain.FuelTransactionDate.Create(Instance<string>());
        }

        private static ProductNumber ProductNumber()
        {
            return Domain.ProductNumber.Create(Instance<string>());
        }

        public static Quantity Quantity()
        {
            return Domain.Quantity.Create(Instance<decimal>());
        }

        public static FuelTransactionCountry Country()
        {
            return Domain.FuelTransactionCountry.Create(Instance<string>());
        }
        public static ItemCount ItemCount()
        {
            return Domain.ItemCount.Create(Instance<int>());
        }

        private static Odometer Odometer()
        {
            return Domain.Odometer.Create(Instance<int>());
        }

        private static CardTypeId CardTypeId()
        {
            return Domain.CardTypeId.Create(Instance<string>());
        }

        private static CardAcceptanceId CardAcceptanceId()
        {
            return Domain.CardAcceptanceId.Create(Instance<string>());
        }

        private static DriverCardNumber DriverCardNumber()
        {
            return Domain.DriverCardNumber.Create(Instance<string>());
        }

        private static VehicleCardNumber VehicleCardNumber()
        {
            return Domain.VehicleCardNumber.Create(Instance<string>());
        }

        private static CustomerNumber CustomerNumber()
        {
            return BuildingBlocks.Domain.CustomerNumber.Create(Instance<string>());
        }

        private static Pump Pump()
        {
            return Domain.Pump.Create(Instance<int>());
        }

        private static SourceETag SourceETag()
        {
            return Domain.SourceETag.Create(Instance<string>());
        }
    }
}