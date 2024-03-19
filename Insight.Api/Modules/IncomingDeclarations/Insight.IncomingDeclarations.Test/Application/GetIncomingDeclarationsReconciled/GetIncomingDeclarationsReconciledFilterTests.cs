using FluentAssertions;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.GetIncomingDeclarationsReconciled;

public class GetIncomingDeclarationsReconciledFilterTests
{
    [Fact]
    public async Task Should_GetReconciledDeclarations_filterOnDatePeriod(
    )
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var relevantIncomingDeclaration = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2020, 06, 14)));
        relevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(relevantIncomingDeclaration);

        var notRelevantIncomingDeclaration = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2000, 06, 14)));
        notRelevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(notRelevantIncomingDeclaration);
        
        DatePeriod datePeriodFilter = DatePeriod.Create(new DateOnly(2010, 06, 14), new DateOnly(2030, 06, 14));

        var paginationParameters = PaginationParameters.Create(0, 2);
        var filterParameters = FilteringParameters.Create(datePeriodFilter, Product.Empty(), Company.Empty(), Supplier.Empty());
        var states = new List<IncomingDeclarationState> { IncomingDeclarationState.Reconciled};

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, SortingParameters.Empty(), filterParameters,  states);
        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(relevantIncomingDeclaration.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(relevantIncomingDeclaration.Id.ToString());
    }

    [Fact]
    public async Task Should_GetReconciledDeclarations_filterOnProduct(
    )
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var relevantIncomingDeclaration = Any.IncomingDeclaration(Product.Create("GreatProduct"));
        relevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(relevantIncomingDeclaration);

        var notRelevantIncomingDeclaration = Any.IncomingDeclaration(Product.Create("NotSoGreat"));
        notRelevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(notRelevantIncomingDeclaration);

        var paginationParameters = PaginationParameters.Create(page: 0, pageSize: 2);
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Create("GreatProduct"), Company.Empty(), Supplier.Empty());
        var states = new List<IncomingDeclarationState> { IncomingDeclarationState.Reconciled };

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, SortingParameters.Empty(), filterParameters, states);
        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(relevantIncomingDeclaration.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(relevantIncomingDeclaration.Id.ToString());
    }
    [Fact]
    public async Task Should_GetReconciledDeclarations_filterOnCompany(
    )
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var relevantIncomingDeclaration = Any.IncomingDeclaration(Company.Create("GreatCompany"));
        relevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(relevantIncomingDeclaration);

        var notRelevantIncomingDeclaration = Any.IncomingDeclaration(Company.Create("NotSoGreat"));
        notRelevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(notRelevantIncomingDeclaration);

        DatePeriod datePeriodFilter = DatePeriod.Create(new DateOnly(2010, 06, 14), new DateOnly(2030, 06, 14));

        var paginationParameters = PaginationParameters.Create(0, 2);
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Create("GreatCompany"), Supplier.Empty());
        var states = new List<IncomingDeclarationState> { IncomingDeclarationState.Reconciled };

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, SortingParameters.Empty(), filterParameters, states);
        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(relevantIncomingDeclaration.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(relevantIncomingDeclaration.Id.ToString());
    }

    [Fact]
    public async Task Should_GetReconciledDeclarations_filterOnSupplier(
    )
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var relevantIncomingDeclaration = Any.IncomingDeclaration(Supplier.Create("GreatSupplier"));
        relevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(relevantIncomingDeclaration);

        var notRelevantIncomingDeclaration = Any.IncomingDeclaration(Supplier.Create("NotSoGreat"));
        notRelevantIncomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
        await incomingDeclarationInMemoryRepository.Add(notRelevantIncomingDeclaration);

        DatePeriod datePeriodFilter = DatePeriod.Create(new DateOnly(2010, 06, 14), new DateOnly(2030, 06, 14));

        var paginationParameters = PaginationParameters.Create(0, 2);
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), Supplier.Create("GreatSupplier"));
        var states = new List<IncomingDeclarationState> { IncomingDeclarationState.Reconciled };

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, SortingParameters.Empty(), filterParameters, states);
        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(relevantIncomingDeclaration.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(relevantIncomingDeclaration.Id.ToString());
    }
}