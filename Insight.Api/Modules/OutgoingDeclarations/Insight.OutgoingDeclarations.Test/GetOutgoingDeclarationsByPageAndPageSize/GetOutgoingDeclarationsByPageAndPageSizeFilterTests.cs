using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize;
using Insight.OutgoingDeclarations.Domain;
using Insight.OutgoingDeclarations.Infrastructure;
using PaginationParameters = Insight.OutgoingDeclarations.Domain.PaginationParameters;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.OutgoingDeclarations.Test.GetOutgoingDeclarationsByPageAndPageSize;

public class GetOutgoingDeclarationsByPageAndPageSizeFilterTests
{
    [Fact]
    public async Task Should_GetOutgoingDeclarations_filterOnDatePeriod()
    {
        // Arrange
        
        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var outgoingDeclarationWithinFilter1 = Any.OutgoingDeclaration(DateOfIssuance.Create(new DateOnly(2020,06, 14)));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclarationWithinFilter1);
        var outgoingDeclarationOutsideFilter = Any.OutgoingDeclaration(DateOfIssuance.Create(new DateOnly(2009, 06, 14)));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclarationOutsideFilter);
        var outgoingDeclarationWithinFilter2 = Any.OutgoingDeclaration(DateOfIssuance.Create(new DateOnly(2010, 06, 14)));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclarationWithinFilter2);
        var outgoingDeclarationWithinFilter3 = Any.OutgoingDeclaration(DateOfIssuance.Create(new DateOnly(2030, 06, 14)));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclarationWithinFilter3);

        DatePeriod datePeriod = DatePeriod.Create(new DateOnly(2010, 06, 14), new DateOnly(2030, 06, 14));

        // Set query parameters
        var filterParameters = FilteringParameters.Create(datePeriod, Product.Empty(), Company.Empty(), CustomerName.Empty());
        var sortingParameters = SortingParameters.Create(true, "DateOfIssuance");

        var query = GetOutgoingDeclarationsByPageAndPageSizeQuery.Create(PaginationParameters.Create(page: 0, pageSize:10), sortingParameters, filterParameters);

        var handler = new GetOutgoingDeclarationsByPageAndPageSizeQueryHandler(outgoingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(3);
        result.HasMoreDeclarations.Should().Be(false);
    }

    [Fact]
    public async Task Should_GetOutgoingDeclarations_filterOn_Product()
    {
        // Arrange
         
        var incomingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();
        
        var outgoingDeclaration1 = Any.OutgoingDeclaration(Product.Create("GreatProduct"));
        await incomingDeclarationInMemoryRepository.Add(outgoingDeclaration1);

        var outgoingDeclaration2 = Any.OutgoingDeclaration(Product.Create("NotSoGreat"));
        await incomingDeclarationInMemoryRepository.Add(outgoingDeclaration2);

        // Set query parameters
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Create("GreatProduct"), Company.Empty(), CustomerName.Empty());
        var sortingParameters = SortingParameters.Create(true, "Product");
        
        var query = GetOutgoingDeclarationsByPageAndPageSizeQuery.Create(PaginationParameters.Create(page: 0, pageSize: 10), sortingParameters, filterParameters);

        var handler = new GetOutgoingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().NotBe(2); 
        result.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(1);
        result.OutgoingDeclarationsByPageAndPageSizeResponse.Single().Product.Should().Be(outgoingDeclaration1.Product.Value);
        result.HasMoreDeclarations.Should().Be(false);
    }

    [Fact]
    public async Task Should_GetOutgoingDeclarations_filterOn_CustomerName()
    {
        // Arrange
        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var outgoingDeclaration1 = Any.OutgoingDeclaration(CustomerName.Create("GreatCustomerName"));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclaration1);

        var outgoingDeclaration2 = Any.OutgoingDeclaration(CustomerName.Create("NoSoGreat"));
        await outgoingDeclarationInMemoryRepository.Add(outgoingDeclaration2);

        // Set query parameters
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), CustomerName.Create("GreatCustomerName"));
        var sortingParameters = SortingParameters.Create(true, "CustomerName");
        var query = GetOutgoingDeclarationsByPageAndPageSizeQuery.Create(PaginationParameters.Create(page: 0, pageSize: 10), sortingParameters, filterParameters);

        var handler = new GetOutgoingDeclarationsByPageAndPageSizeQueryHandler(outgoingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }
}