using FluentAssertions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain;
using Insight.Customers.Infrastructure.Repositories;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Globalization;
using Xunit;

namespace Insight.Services.AllocationEngine.Service.AllocationEngine.Tests
{
    public class GetSuggestionsTests
    {
        [Fact]
        public async Task ShouldReturnEmptyListWhenNoTransactions()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";

            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);
            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitOfWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldNotReturnSuggestionsWhenNoValidIncomingDeclarationsExist()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";
            var ghgEmissionSaving = GHGEmissionSaving.Create(0.5M);

            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate.AddDays(-1)), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), ghgEmissionSaving, IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitOfWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnSuggestionsWhenValidIncomingDeclarationsExist()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";
            var ghgEmissionSaving = GHGEmissionSaving.Create(0.5M);

            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), ghgEmissionSaving, IncomingDeclarations.Domain.Incoming.Quantity.Create(100));

            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled); 
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitOfWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().NotBeEmpty();
        }

        [Fact]
        public async Task IncomingDeclarationsWithTheWrongStateShouldNotBeReturned()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";
            var ghgEmissionSaving = GHGEmissionSaving.Create(0.5M);

            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), ghgEmissionSaving, IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            var incomingDeclaration2 = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), ghgEmissionSaving, IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Temporary);
            incomingDeclaration2.SetIncomingDeclarationState(IncomingDeclarationState.New);
            await incomingDeclarationRepository.Add(incomingDeclaration);
            await incomingDeclarationRepository.Add(incomingDeclaration2);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitOfWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().BeEmpty();
        }


        [Fact]
        public async Task MakeSureHighestGHGEmissionSavingDeclarationsAreReturnedFirst()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            var incomingDeclaration2 = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.75M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration2.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            var incomingDeclaration3 = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.51M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration3.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            await incomingDeclarationRepository.Add(incomingDeclaration);
            await incomingDeclarationRepository.Add(incomingDeclaration2);
            await incomingDeclarationRepository.Add(incomingDeclaration3);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitOfWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.First().GHGReduction.Should().Be(0.75M);
        }

        [Fact]
        public async Task MakeSureWarningsArePresentForNotAllowedRawMaterial()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            customer.SetAllowedRawMaterials(AllowedRawMaterials.Create(new Dictionary<string, bool>() { { "KittenJuice", true } })); // Positive listed.
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().AllSatisfy(c => c.HasWarnings.Should().BeTrue());
        }

        [Fact]
        public async Task MakeSureNoWarningsArePresentForAllowedRawMaterial()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            customer.SetAllowedRawMaterials(AllowedRawMaterials.Create(new Dictionary<string, bool>() { { "KittenJuice", false } })); // Negative listed.
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().AllSatisfy(c => c.HasWarnings.Should().BeFalse());
        }

        [Fact]
        public async Task FullyAllocatedDeclarationMustNotBeReturned()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            incomingDeclaration.Allocations.Value.Add(AllocationId.Empty().Value, incomingDeclaration.Quantity.Value);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().BeEmpty();
        }

        [Fact]
        public async Task DeclarationWithCapacityMustBeReturned()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            incomingDeclaration.Allocations.Value.Add(AllocationId.Empty().Value, incomingDeclaration.Quantity.Value * 0.5M);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeclarationsThatHaveNotBeenApprovedShouldNotBeReturned()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.New);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().BeEmpty();
        }

        [Fact]
        public async Task DeclarationsWithLowerReductionThanTargetMustContainWarning()
        {
            var incomingDeclarationRepository = new IncomingDeclarationInMemoryRepository();
            var customerRepository = new CustomerInMemoryRepository();
            var allocationDraftRepositoryMock = new Mock<IAllocationDraftRepository>();
            var fuelTransactionsAllocationRepositoryMock = new Mock<IFuelTransactionsAllocationRepository>();
            var incomingDeclarationAllocationRepositoryMock = new Mock<IIncomingDeclarationAllocationRepository>();
            var unitofWorkMock = new Mock<IUnitOfWork>();
            var customerId = FuelTransactionCustomerId.Create(Guid.NewGuid());
            var startDate = DateOnly.Parse("2021-01-01", CultureInfo.InvariantCulture);
            var endDate = DateOnly.Parse("2021-01-31", CultureInfo.InvariantCulture);
            var product = "product";
            var country = "country";
            var location = "location";


            var customer = Customers.Tests.Any.Customer(customerId.Value);
            customer.SetCO2Target(CO2Target.Create(0.8M));
            await customerRepository.Add(customer);

            var incomingDeclaration = IncomingDeclarations.Test.Any.IncomingDeclaration(DateOfDispatch.Create(startDate), Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), GHGEmissionSaving.Create(0.5M), IncomingDeclarations.Domain.Incoming.Quantity.Create(100));
            incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
            await incomingDeclarationRepository.Add(incomingDeclaration);

            var suggestionRequest = new SuggestionRequest(customerId, startDate, endDate, Product.Create(product), Country.Create(country), PlaceOfDispatch.Create(location), true, OrderByProperty.Create("company"));
            var logger = NullLogger<AllocationService>.Instance;
            var allocationService = new AllocationService(incomingDeclarationRepository, customerRepository, allocationDraftRepositoryMock.Object, fuelTransactionsAllocationRepositoryMock.Object, incomingDeclarationAllocationRepositoryMock.Object, unitofWorkMock.Object, logger);

            var suggestions = await allocationService.GetSuggestionsAsync(suggestionRequest, CancellationToken.None);

            suggestions.Should().NotBeEmpty();
            suggestions.Should().OnlyContain(c => c.HasWarnings);
        }
    }
}
