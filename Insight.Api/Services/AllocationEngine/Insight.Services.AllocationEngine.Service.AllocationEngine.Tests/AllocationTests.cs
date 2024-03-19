using FluentAssertions;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Customers.Infrastructure.Repositories;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Infrastructure.Allocation;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Marten;
using Marten.PLv8;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Globalization;
using Insight.BuildingBlocks.Domain;
using Testcontainers.PostgreSql;
using Xunit;
using Quantity = Insight.FuelTransactions.Domain.Quantity;

namespace Insight.Services.AllocationEngine.Service.AllocationEngine.Tests
{
    public class AllocationTests
    {
        // Todo: Re-use db.
        private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
                   .WithDatabase("db")
                   .WithUsername("postgres")
                   .WithPassword("postgres")
                   .WithImage("clkao/postgres-plv8:11-2")
                   .Build();

        [Fact]
        public async Task FuelTransactionsShouldHaveDraftAllocationIdPresentAfterDraftAllocation()
        {
            await postgreSqlContainer.StartAsync();

            var connString = $"{postgreSqlContainer.GetConnectionString()};IncludeErrorDetail=true";

            var configureMarten = new FuelTransactions.Infrastructure.Configuration.ConfigureMarten();

            var store = DocumentStore.For(opts =>
            {   
                opts.Connection(connString);
                opts.UseJavascriptTransformsAndPatching();
                opts.Serializer(MartenRegistration.GetJsonNetSerializer());
                opts.Logger(new ConsoleMartenLogger());
                configureMarten.Configure(new Mock<IServiceProvider>().Object, opts);
            });

            var logger = NullLogger<AllocationService>.Instance;
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var allocationDraft = AllocationDraft.Create();

            allocationDraftRepositoryMock.Setup(c => c.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);

            var ds = store.LightweightSession();            

            var fuelTransactionsAllocationRepository = new FuelTransactionsAllocationRepository(ds);
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var productNumber = "456";
            var product = "B100";
            var country = "Denmark";
            var stationNumber = "1234";
            var stationName = "My Gas Station";
            var locationId = $"{country}:{stationNumber}:{stationName}";
            var companyName = "CompanyName";

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate.AddDays(1)), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(stationName), GHGEmissionSaving.Create(0.9M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            await incomingDeclarationRepository.Add(incomingDeclaration, CancellationToken.None);

            var ft = FuelTransactions.Tests.Any.FuelTransaction(startDate.AddDays(1).ToDateTime(TimeOnly.FromDateTime(DateTime.Now)),
                                                                customerId,
                                                                CustomerNumber.Create("123"),
                                                                CustomerName.Create("CustomerName"),
                                                                CustomerType.Create("B2B"),
                                                                CustomerSegment.Create("B2B"),
                                                                ProductNumber.Create(productNumber),
                                                                ProductName.Create(product),
                                                                FuelTransactionCountry.Create(country),
                                                                StationName.Create(stationName),
                                                                StationNumber.Create(stationNumber),
                                                                Quantity.Create(100),
                                                                Location.Create("EXTERNAL"),
                                                                CompanyName.Create(companyName));
            ds.Store(ft);
            await ds.SaveChangesAsync();
            ft.DraftAllocationId.Value.Should().BeEmpty();
            var uow = new MartenUnitOfWork(ds);
            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);


            var fuelTransactionsBatch = FuelTransactionsBatch.Create(customerId,
                                                                     startDate,
                                                                     endDate,
                                                                     ProductNumber.Create(productNumber),
                                                                     FuelTransactionCountry.Create(country),
                                                                     StationName.Create(stationName),
                                                                     ProductName.Create(product),
                                                                     LocationId.Create(locationId)
                                                                     );

            var manualAllocationAssignments = ManualAllocationAssignments.Create(fuelTransactionsBatch,
                new AllocationAssignment[] {
                    AllocationAssignment.Create(incomingDeclaration.IncomingDeclarationId,
                    IncomingDeclarations.Domain.Incoming.Quantity.Create(ft.Quantity.Value)) });

            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();

            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepository, incomingDeclarationAllocationRepositoryMock.Object, uow, logger);
            await allocationService.ManuallyAllocate(manualAllocationAssignments, CancellationToken.None);

            var ftFromDatabase = await ds.LoadAsync<FuelTransaction>(ft.FuelTransactionId.Value);
            ftFromDatabase.Should().NotBeNull();
            ftFromDatabase!.DraftAllocationId.Value.Should().NotBeEmpty();
            allocationDraft.Allocations.Should().HaveCount(1);
            await postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task FuelTransactionsShouldNotHaveDraftAllocationIdPresentAfterDraftIsCleared()
        {
            await postgreSqlContainer.StartAsync();

            var connString = $"{postgreSqlContainer.GetConnectionString()};IncludeErrorDetail=true";
            var configureMarten = new FuelTransactions.Infrastructure.Configuration.ConfigureMarten();

            var store = DocumentStore.For(opts =>
            {
                opts.Connection(connString);
                opts.UseJavascriptTransformsAndPatching();
                opts.Serializer(MartenRegistration.GetJsonNetSerializer());
                opts.Logger(new ConsoleMartenLogger());
                configureMarten.Configure(new Mock<IServiceProvider>().Object, opts);
            });

            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var allocationDraft = AllocationDraft.Create();
            var logger = NullLogger<AllocationService>.Instance;
            allocationDraftRepositoryMock.Setup(c => c.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);

            var ds = store.LightweightSession();

            var fuelTransactionsAllocationRepository = new FuelTransactionsAllocationRepository(ds);
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var productNumber = "456";
            var product = "B100";
            var country = "Denmark";
            var stationNumber = "1234";
            var stationName = "My Gas Station";
            var locationId = $"{country}:{stationNumber}:{stationName}";

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate.AddDays(2)), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(stationName), GHGEmissionSaving.Create(0.9M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            await incomingDeclarationRepository.Add(incomingDeclaration, CancellationToken.None);

            var ft = FuelTransactions.Tests.Any.FuelTransaction(startDate.AddDays(1).ToDateTime(TimeOnly.FromDateTime(DateTime.Now)),
                                                                customerId,
                                                                CustomerNumber.Create("123"),
                                                                CustomerName.Create("CustomerName"),
                                                                CustomerType.Create("B2B"),
                                                                CustomerSegment.Create("B2B"),
                                                                ProductNumber.Create(productNumber),
                                                                ProductName.Create(product),
                                                                FuelTransactionCountry.Create(country),
                                                                StationName.Create(stationName),
                                                                StationNumber.Create(stationNumber),
                                                                Quantity.Create(100),
                                                                Location.Create("EXTERNAL"),
                                                                CompanyName.Create("CompanyName"));
            ds.Store(ft);
            await ds.SaveChangesAsync();
            ft.DraftAllocationId.Value.Should().BeEmpty();
            var uow = new MartenUnitOfWork(ds);
            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var fuelTransactionsBatch = FuelTransactionsBatch.Create(customerId,
                                                                     startDate,
                                                                     endDate,
                                                                     ProductNumber.Create(productNumber),
                                                                     FuelTransactionCountry.Create(country),
                                                                     StationName.Create(stationName),
                                                                     ProductName.Create(product),
                                                                     LocationId.Create(locationId));

            var manualAllocationAssignments = ManualAllocationAssignments.Create(fuelTransactionsBatch,
                new AllocationAssignment[] {
                    AllocationAssignment.Create(incomingDeclaration.IncomingDeclarationId,
                    IncomingDeclarations.Domain.Incoming.Quantity.Create(ft.Quantity.Value)) });

            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();

            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepository, incomingDeclarationAllocationRepositoryMock.Object, uow, logger);
            await allocationService.ManuallyAllocate(manualAllocationAssignments, CancellationToken.None);

            var ftFromDatabase = await ds.LoadAsync<FuelTransaction>(ft.FuelTransactionId.Value);
            ftFromDatabase.Should().NotBeNull();
            ftFromDatabase!.DraftAllocationId.Value.Should().NotBeEmpty();
            allocationDraft.Allocations.Should().HaveCount(1);

            // Clear
            await allocationService.ClearDraftAsync(CancellationToken.None);

            ftFromDatabase = await ds.LoadAsync<FuelTransaction>(ft.FuelTransactionId.Value);
            ftFromDatabase.Should().NotBeNull();
            ftFromDatabase!.DraftAllocationId.Value.Should().BeEmpty();

            await postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task ClearingDraftShouldDeleteDraft()
        {
            await postgreSqlContainer.StartAsync();

            var connString = $"{postgreSqlContainer.GetConnectionString()};IncludeErrorDetail=true";

            var store = DocumentStore.For(opts =>
            {
                opts.Connection(connString);
                opts.UseJavascriptTransformsAndPatching();
                opts.Serializer(MartenRegistration.GetJsonNetSerializer());
                opts.Logger(new ConsoleMartenLogger());
            });


            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var allocationDraft = AllocationDraft.Create();

            allocationDraftRepositoryMock.Setup(c => c.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);
            allocationDraftRepositoryMock.Setup(c => c.Delete(It.IsAny<AllocationDraft>(), It.IsAny<CancellationToken>())).Verifiable();
            var ds = store.LightweightSession();

            var fuelTransactionsAllocationRepository = new FuelTransactionsAllocationRepository(ds);
            var uow = new MartenUnitOfWork(ds);

            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepository, incomingDeclarationAllocationRepositoryMock.Object, uow, logger);

            await allocationService.ClearDraftAsync(CancellationToken.None);

            allocationDraftRepositoryMock.VerifyAll();

            await postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
