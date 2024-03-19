using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;

namespace Insight.Services.BusinessCentralConnector.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        private static string GetRandomValidProductName()
        {
            var validProductNames = new List<string> { "HVO DIESEL", "B100" };
            return validProductNames.OrderBy(c => Guid.NewGuid()).First();
        }

        public static FuelTransaction FuelTransaction()
        {
            return FuelTransactions.Domain.FuelTransaction.Create(FuelTransactionId.Create(Instance<Guid>()),
                FuelTransactionPosSystem.Tokheim,
                StationNumber.Create(Instance<string>()),
                StationName.Create(Instance<string>()),
                FuelTransactionDate.Create("2023-12-15"),
                FuelTransactionTime.Create("01:48:48"),
                ProductNumber.Create(Instance<string>()),
                ProductName.Create(GetRandomValidProductName()),
                Quantity.Create(Instance<decimal>()),
                Odometer.Create(Instance<int>()),
                DriverCardNumber.Create(Instance<string>()),
                VehicleCardNumber.Create(Instance<string>()),
                CustomerNumber.Create(Instance<string>()),
                CustomerName.Create(Instance<string>()),
                SourceETag.Create(Instance<string>()),
                FuelTransactionCountry.Create(Instance<string>()), 
                SourceSystemPropertyBag.Create(Instance<string>()),
                SourceSystemId.Create(Instance<Guid>()),
                Location.Create(Instance<string>()),
                CustomerType.Create(Instance<string>()),
                CustomerSegment.Create(Instance<string>()), 
                CompanyName.Create(Instance<string>()),
                AccountNumber.Create(Instance<string>()),
                AccountName.Create(Instance<string>()),
                AccountCustomerId.Create(Instance<Guid>()),
                ProductDescription.Create(Instance<string>()),
                ShipToLocation.Create(Instance<string>()),
                Driver.Create(Instance<string>()));
        }
    }
}
