using FluentAssertions;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.GetIncomingDeclarationsByPageAndPageSize;

public class GetIncomingDeclarationsByPageAndPageSizeFilterTests
{
    [Fact]
    public async Task Should_GetIncomingDeclarations_filterOnDatePeriod()
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var incomingDeclarationWithinFilter1 = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2020,06, 14)));
        incomingDeclarationWithinFilter1.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclarationWithinFilter1);
        
        var incomingDeclarationOutsideFilter = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2009, 06, 14)));
        incomingDeclarationOutsideFilter.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclarationOutsideFilter);
        
        var incomingDeclarationWithinFilter2 = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2010, 06, 14)));
        incomingDeclarationWithinFilter2.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclarationWithinFilter2);
        
        var incomingDeclarationWithinFilter3 = Any.IncomingDeclaration(DateOfDispatch.Create(new DateOnly(2030, 06, 14)));
        incomingDeclarationWithinFilter3.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclarationWithinFilter3);

        var datePeriod = DatePeriod.Create(new DateOnly(2010, 06, 14), new DateOnly(2030, 06, 14));
        var filterParameters = FilteringParameters.Create(datePeriod, Product.Empty(), Company.Empty(), Supplier.Empty());
        var sortingParameters = SortingParameters.Create(true, "DateOfDispatch");

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(
            PaginationParameters.Create(page: 0, pageSize:10), sortingParameters, filterParameters,
            new List<IncomingDeclarationState>() { IncomingDeclarationState.New });

        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(3);
    }

    [Fact]
    public async Task Should_GetIncomingDeclarations_filterOn_Product()
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var incomingDeclaration1 = Any.IncomingDeclaration(Product.Create("GreatProduct"));
        incomingDeclaration1.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration1);

        var incomingDeclaration2 = Any.IncomingDeclaration(Product.Create("NotSoGreat"));
        incomingDeclaration2.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration2);

        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Create("GreatProduct"), Company.Empty(), Supplier.Empty());
        var sortingParameters = SortingParameters.Create(true, "Product");
        
        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(
            PaginationParameters.Create(page: 0, pageSize: 10), sortingParameters, filterParameters,
            new List<IncomingDeclarationState>(){ IncomingDeclarationState.New });

        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(incomingDeclaration1.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(incomingDeclaration1.Id.ToString());
    }

    [Fact]
    public async Task Should_GetIncomingDeclarations_filterOn_Company()
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var incomingDeclaration1 = Any.IncomingDeclaration(Company.Create("GreatCompany"));
        incomingDeclaration1.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration1);

        var incomingDeclaration2 = Any.IncomingDeclaration(Company.Create("NotSoGreat"));
        incomingDeclaration2.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration2);

        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Create("GreatCompany"), Supplier.Empty());
        var sortingParameters = SortingParameters.Create(true, "Company");

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(
            PaginationParameters.Create(page: 0, pageSize: 10), sortingParameters, filterParameters,
            new List<IncomingDeclarationState>() { IncomingDeclarationState.New });

        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(incomingDeclaration1.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(incomingDeclaration1.Id.ToString());
    }

    [Fact]
    public async Task Should_GetIncomingDeclarations_filterOn_Supplier()
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var incomingDeclaration1 = Any.IncomingDeclaration(Supplier.Create("GreatSupplier"));
        incomingDeclaration1.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration1);

        var incomingDeclaration2 = Any.IncomingDeclaration(Supplier.Create("NoSoGreat"));
        incomingDeclaration2.SetIncomingDeclarationState(IncomingDeclarationState.New);
        await incomingDeclarationInMemoryRepository.Add(incomingDeclaration2);

        var filterParameters = FilteringParameters.Create(
            DatePeriod.Empty(), Product.Empty(), Company.Empty(), Supplier.Create("GreatSupplier"));
        var sortingParameters = SortingParameters.Create(true, "Supplier");
        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(
            PaginationParameters.Create(page: 0, pageSize: 10), sortingParameters, filterParameters,
            new List<IncomingDeclarationState>() { IncomingDeclarationState.New });

        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().NotBe(2);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(1);
        result.IncomingDeclarationsByPageAndPageSize.Single().Product.Should().Be(incomingDeclaration1.Product.Value);
        result.IncomingDeclarationsByPageAndPageSize.Single().Id.Should().Be(incomingDeclaration1.Id.ToString());
    }
}