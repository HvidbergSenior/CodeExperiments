using FluentAssertions;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Customers.Infrastructure.Repositories;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Infrastructure;
using Insight.FuelTransactions.Infrastructure.Allocation;
using Insight.FuelTransactions.Infrastructure.Configuration;
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

namespace Insight.Services.AllocationEngine.Service.AllocationEngine.Tests
{
    public class AutomaticAllocationTests
    {
        private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
                   .WithDatabase("db")
                   .WithUsername("postgres")
                   .WithPassword("postgres")
                   .WithImage("clkao/postgres-plv8:11-2")
                   .Build();


        [Fact]
        public async Task AutomaticAllocateWithNoWarnings()
        {
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);

            var transactionCount = 100;
            var transactionVolume = 100;
            var incomingDeclarationVolume = transactionVolume * transactionCount;

            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            await incomingDeclarationRepository.AddIncomingDeclarations(startDate,
                                                                        endDate,
                                                                        Product.Create("HVO DIESEL"),
                                                                        Country.Create("Sweden"),
                                                                        PlaceOfDispatch.Create("109:Uppsala"),
                                                                        IncomingDeclarations.Domain.Incoming.Quantity.Create(incomingDeclarationVolume),
                                                                        IncomingDeclarationState.Reconciled,
                                                                        3, minGhgr: 0.8f, maxGhgr: 0.89f);
            var customerRepository = new CustomerInMemoryRepository();

            var addedCustomers = await customerRepository.AddCustomers(3);

            var fuelTransactionsRepository = new FuelTransactionsInMemoryRepository();

            foreach(var customer in addedCustomers)
            {
                await fuelTransactionsRepository.AddFuelTransactions(FuelTransactionCustomerId.Create(customer.Value),
                                                                     CustomerNumber.Create("123"),
                                                                     CustomerName.Create("CustomerName"),
                                                                     CustomerSegment.Create("Segment"),
                                                                     CustomerType.Create("Type"),
                                                                     startDate,
                                                                     endDate,
                                                                     ProductNumber.Create("451"),
                                                                     ProductName.Create("HVO DIESEL"),
                                                                     CompanyName.Create("CompanyName"), 
                                                                     FuelTransactionCountry.Create("Sweden"),
                                                                     StationName.Create("Uppsala"),
                                                                     StationNumber.Create("109"),
                                                                     FuelTransactions.Domain.Quantity.Create(transactionVolume),
                                                                     Location.Create("EXTERNAL"),
                                                                     transactionCount);
            }

            var allocationDraft = AllocationDraft.Create();

            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();

            allocationDraftRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);
            var unitofWorkMock = new Mock<IUnitOfWork>();
            
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsRepository, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);
            
            await allocationService.AutoAllocate(startDate, endDate, FilterProductName.Empty(), FilterCompanyName.Empty(), FilterCustomerName.Empty(), CancellationToken.None);
            allocationDraft.Allocations.Should().HaveCount(3);

            var allDeclarations = await incomingDeclarationRepository.GetAllByPaging(1, 100);

            allDeclarations.Should().OnlyContain(c => c.Allocations.Value.Any());
            allDeclarations.Should().OnlyContain(c => c.Allocations.TotalAllocatedVolume == incomingDeclarationVolume);
            allDeclarations.Should().OnlyContain(c => c.RemainingVolume == 0);

            fuelTransactionsRepository.Values.ToList().Should().OnlyContain(c => c.Value.DraftAllocationId.Value != Guid.Empty);
            allocationDraft.Allocations.Should().OnlyContain(c => !c.Value.Warnings.Any());
        }

        [Fact]
        public async Task AutomaticAllocateWithWarnings()
        {
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-12-31", CultureInfo.InvariantCulture);

            var transactionCount = 100;
            var transactionVolume = 100;
            var incomingDeclarationVolume = transactionVolume * transactionCount;

            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            await incomingDeclarationRepository.AddIncomingDeclarations(startDate,
                                                                        endDate,
                                                                        Product.Create("HVO DIESEL"),
                                                                        Country.Create("Sweden"),
                                                                        PlaceOfDispatch.Create("109:Uppsala"),
                                                                        IncomingDeclarations.Domain.Incoming.Quantity.Create(incomingDeclarationVolume),
                                                                        IncomingDeclarationState.Reconciled,
                                                                        3, minGhgr:0.5f, maxGhgr:0.7f);
            var customerRepository = new CustomerInMemoryRepository();

            var addedCustomers = await customerRepository.AddCustomers(3,  minCo2Target:0.8f, maxCo2Target: 0.9f);

            var fuelTransactionsRepository = new FuelTransactionsInMemoryRepository();

            foreach (var customer in addedCustomers)
            {
                await fuelTransactionsRepository.AddFuelTransactions(FuelTransactionCustomerId.Create(customer.Value),
                                                                     CustomerNumber.Create("123"),
                                                                     CustomerName.Create("CustomerName"),
                                                                     CustomerSegment.Create("Segment"),
                                                                     CustomerType.Create("Type"),
                                                                     startDate,
                                                                     endDate,
                                                                     ProductNumber.Create("451"),
                                                                     ProductName.Create("HVO DIESEL"),
                                                                     CompanyName.Create("CompanyName"),
                                                                     FuelTransactionCountry.Create("Sweden"),
                                                                     StationName.Create("Uppsala"),
                                                                     StationNumber.Create("109"),
                                                                     FuelTransactions.Domain.Quantity.Create(transactionVolume),
                                                                     Location.Create("TheLocation"),
                                                                     transactionCount);
            }

            var allocationDraft = AllocationDraft.Create();

            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            allocationDraftRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);
            var unitofWorkMock = new Mock<IUnitOfWork>();

            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsRepository, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);            

            await allocationService.AutoAllocate(startDate, endDate, FilterProductName.Empty(), FilterCompanyName.Empty(), FilterCustomerName.Empty(), CancellationToken.None);
            allocationDraft.Allocations.Should().HaveCount(3);
 
            var allDeclarations = await incomingDeclarationRepository.GetAllByPaging(1, 100);

            allDeclarations.Should().OnlyContain(c => c.Allocations.Value.Any());
            allDeclarations.Should().OnlyContain(c => c.Allocations.TotalAllocatedVolume == incomingDeclarationVolume);
            allDeclarations.Should().OnlyContain(c => c.RemainingVolume == 0);
            fuelTransactionsRepository.Values.ToList().Should().OnlyContain(c => c.Value.DraftAllocationId.Value != Guid.Empty);
            allocationDraft.Allocations.Should().OnlyContain(c => c.Value.Warnings.Any());
        }

        [Fact]
        public async Task AutomaticAllocateWithInsufficientIncomingDeclarationCapacityShouldOnlyPartiallyAllocateFuelTransactions()
        {
            await postgreSqlContainer.StartAsync();

            var connString = $"{postgreSqlContainer.GetConnectionString()};IncludeErrorDetail=true";

            var spMock = new Mock<IServiceProvider>();
            var cm = new ConfigureMarten();

            var store = DocumentStore.For(opts =>
            {
                opts.Connection(connString);
                opts.UseJavascriptTransformsAndPatching();
                opts.Serializer(MartenRegistration.GetJsonNetSerializer());
                opts.Logger(new ConsoleMartenLogger());
                cm.Configure(spMock.Object, opts);
            });


            var ds = store.IdentitySession();

            await ds.Query<FuelTransaction>().ToListAsync();

            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);

            var transactionCount = 10;
            var transactionVolume = 100;
            var incomingDeclarationVolume = (transactionVolume * transactionCount) - (transactionVolume * 2); // All but the last two.

            var entityEventPublisherMock = new Mock<IEntityEventsPublisher>();
            var incomingDeclarationRepository = new IncomingDeclarationRepository(ds, entityEventPublisherMock.Object);
            await incomingDeclarationRepository.AddIncomingDeclarations(startDate,
                                                                        endDate,
                                                                        Product.Create("HVO DIESEL"),
                                                                        Country.Create("Sweden"),
                                                                        PlaceOfDispatch.Create("109:Uppsala"),
                                                                        IncomingDeclarations.Domain.Incoming.Quantity.Create(incomingDeclarationVolume),
                                                                        IncomingDeclarationState.Reconciled,
                                                                        1, minGhgr: 0.5f, maxGhgr: 0.7f);

            await incomingDeclarationRepository.SaveChanges();

            var customerRepository = new CustomerInMemoryRepository();

            var addedCustomers = await customerRepository.AddCustomers(1);

            var fuelTransactionsRepository = new FuelTransactionsRepository(ds, entityEventPublisherMock.Object);
            var fuelTransactionsAllocationRepository = new FuelTransactionsAllocationRepository(ds);
            
            foreach (var customer in addedCustomers)
            {
                await fuelTransactionsRepository.AddFuelTransactions(FuelTransactionCustomerId.Create(customer.Value),
                                                                     CustomerNumber.Create("123"),
                                                                     CustomerName.Create("CustomerName"),
                                                                     CustomerSegment.Create("Segment"),
                                                                     CustomerType.Create("Type"),
                                                                     startDate,
                                                                     endDate,
                                                                     ProductNumber.Create("451"),
                                                                     ProductName.Create("HVO DIESEL"),
                                                                     CompanyName.Create("CompanyName"), 
                                                                     FuelTransactionCountry.Create("Sweden"),
                                                                     StationName.Create("Uppsala"),
                                                                     StationNumber.Create("109"),
                                                                     FuelTransactions.Domain.Quantity.Create(transactionVolume),
                                                                     Location.Create("EXTERNAL"),
                                                                     transactionCount);
            }
            
            await fuelTransactionsRepository.SaveChanges();
            var allocationDraft = AllocationDraft.Create();

            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            allocationDraftRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(allocationDraft);
            var uow = new MartenUnitOfWork(ds);
            
            await uow.Commit(CancellationToken.None);
            
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepository, incomingDeclarationAllocationRepositoryMock.Object, uow, logger);            

            await allocationService.AutoAllocate(startDate, endDate, FilterProductName.Empty(), FilterCompanyName.Empty(), FilterCustomerName.Empty(), CancellationToken.None);

            ds.EjectAllOfType(typeof(FuelTransaction));
            ds.EjectAllOfType(typeof(IncomingDeclaration));

            allocationDraft.Allocations.Should().HaveCount(1);
            var allIncomingDeclarations = await incomingDeclarationRepository.GetAllByPaging(1, 10);

            allIncomingDeclarations.Should().OnlyContain(c => c.Allocations.Value.Any());
            allIncomingDeclarations.Should().OnlyContain(c => c.Allocations.TotalAllocatedVolume == incomingDeclarationVolume);
            allIncomingDeclarations.Should().OnlyContain(c => c.RemainingVolume == 0);
            
            
            var allFuelTransactions = await fuelTransactionsRepository.GetAllByPaging(1, 200);            
            
            allFuelTransactions.Where(c=> c.DraftAllocationId.Value == Guid.Empty).Should().HaveCount(2);
            allFuelTransactions.Where(c=> c.DraftAllocationId.Value != Guid.Empty).Should().HaveCount(8);
            await postgreSqlContainer.DisposeAsync().AsTask();
        }

    }
}
