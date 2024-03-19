using System.Globalization;
using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Serialization;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Infrastructure;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Insight.Tests.End2End.TestData
{
    public static class FuelTransactionsTestData
    {
        private static string GetRandomValidProductName()
        {
            var validProductNames = new List<string> { "HVO DIESEL", "B100" };
            return validProductNames.OrderBy(c => Guid.NewGuid()).First();
        }

        public static async Task<IEnumerable<FuelTransaction>> SeedWithOutgoingFuelTransactions(int count, WebAppFixture fixture, FilteringParameters filteringParameters, bool useExistingTransactionsIfAny = true, Guid? customerId = null)
        {
            Fixture autoFixture = new();

            var sessionFactory = (ISessionFactory)fixture.AppFactory.Services.GetRequiredService(typeof(ISessionFactory))!;
            var entityEventsPublisher = (IEntityEventsPublisher)fixture.AppFactory.Services.GetRequiredService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);

            var fuelTransactionsRepository = new FuelTransactionsRepository(documentSession, entityEventsPublisher);

            var fuelTransactions = new List<FuelTransaction>();

            var anyTransactions = false;

            if (useExistingTransactionsIfAny)
            {
                anyTransactions = await fuelTransactionsRepository.AnyAsync(CancellationToken.None);
            }

            RandomDateTimeSequenceGenerator generator = new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-5), maxDate: DateTime.Now);
            if (filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                generator = new RandomDateTimeSequenceGenerator(minDate: filteringParameters.DatePeriod.StartDate.ToDateTime(TimeOnly.MinValue), maxDate: filteringParameters.DatePeriod.EndDate.ToDateTime(TimeOnly.MaxValue));
            }

            if (!anyTransactions)
            {
                for (var i = 0; i < count; i++)
                {
                    autoFixture.Customizations.Add(generator);
                    var date = autoFixture.Create<DateTime>();

                    var productName = filteringParameters.ProductName.Value.IsNullOrEmpty() ? ProductName.Create(GetRandomValidProductName()) : ProductName.Create(filteringParameters.ProductName.Value);

                    var ft = FuelTransaction.Create(
                        FuelTransactionId.Create(autoFixture.Create<Guid>()),
                        FuelTransactionPosSystem.Tokheim,
                        StationNumber.Create(autoFixture.Create<string>()),
                        StationName.Create(autoFixture.Create<string>()),
                        FuelTransactionDate.Create(DateOnly.FromDateTime(date).ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture)),
                        FuelTransactionTime.Create(TimeOnly.FromDateTime(date).ToString("HH:mm:ss", CultureInfo.InvariantCulture)),
                        ProductNumber.Create(autoFixture.Create<string>()),
                        productName,
                        Quantity.Create(autoFixture.Create<decimal>()),
                        Odometer.Create(autoFixture.Create<int>()),
                        DriverCardNumber.Create(autoFixture.Create<string>()),
                        VehicleCardNumber.Create(autoFixture.Create<string>()),
                        BuildingBlocks.Domain.CustomerNumber.Create(autoFixture.Create<string>()),
                        filteringParameters.CustomerName.Value.IsNullOrEmpty() ? BuildingBlocks.Domain.CustomerName.Create(autoFixture.Create<string>()) : BuildingBlocks.Domain.CustomerName.Create(filteringParameters.CustomerName.Value),
                        SourceETag.Create(autoFixture.Create<string>()),
                        FuelTransactionCountry.Create(autoFixture.Create<string>()),
                        SourceSystemPropertyBag.Create(autoFixture.Create<string>()),
                        SourceSystemId.Create(autoFixture.Create<Guid>()),
                        Location.Create(autoFixture.Create<string>()),
                        CustomerType.Create(autoFixture.Create<string>()),
                        CustomerSegment.Create(autoFixture.Create<string>()),
                        filteringParameters.CompanyName.Value.IsNullOrEmpty() ? CompanyName.Create(autoFixture.Create<string>()) : CompanyName.Create(filteringParameters.CompanyName.Value),
                        AccountNumber.Create(autoFixture.Create<string>()),
                        filteringParameters.CustomerName.Value.IsNullOrEmpty() ? AccountName.Create(autoFixture.Create<string>()) : AccountName.Create(filteringParameters.CustomerName.Value),
                        AccountCustomerId.Create(autoFixture.Create<Guid>()),
                        ProductDescription.Create(productName.Value),
                        ShipToLocation.Create(autoFixture.Create<string>()),
                        Driver.Create(autoFixture.Create<string>()));

                    if (customerId.HasValue)
                    {
                        ft.SetCustomerId(FuelTransactionCustomerId.Create(customerId.Value));
                    }

                    fuelTransactions.Add(ft);
                }

                await fuelTransactionsRepository.Add(fuelTransactions);
                await fuelTransactionsRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }
            else
            {
                fuelTransactions = (await fuelTransactionsRepository.GetAllByPaging(1, count, CancellationToken.None)).ToList();
            }

            return fuelTransactions;
        }
    }
}
