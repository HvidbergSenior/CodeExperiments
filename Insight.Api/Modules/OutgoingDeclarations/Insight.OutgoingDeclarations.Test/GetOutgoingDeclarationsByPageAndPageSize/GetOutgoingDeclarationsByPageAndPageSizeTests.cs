using FluentAssertions;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize;
using Insight.OutgoingDeclarations.Domain;
using Insight.OutgoingDeclarations.Infrastructure;

namespace Insight.OutgoingDeclarations.Test.GetOutgoingDeclarationsByPageAndPageSize;

public class GetOutgoingDeclarationsByPageAndPageSizeTests
{
    [Fact]
    public async Task Should_get_outgoing_declarations_by_page_and_pagesize()
    {
        //Arrange
       var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();
        const int totalAmountOfDeclarations = 10;
        //page is Skip
        const int page = 2;
        //pageSize is Take
        const int pageSize = 5;
        CreateOutgoingDeclarations(totalAmountOfDeclarations, outgoingDeclarationInMemoryRepository);
        var paginationParameters = PaginationParameters.Create(page, pageSize);
        var sortingParameters = SortingParameters.Create(true, "Company");
        var filteringParameters = Any.FilteringParameters();
        var query = GetOutgoingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, sortingParameters, filteringParameters);
        var handler = new GetOutgoingDeclarationsByPageAndPageSizeQueryHandler(outgoingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
    }
    private static async void CreateOutgoingDeclarations(int amount, OutgoingDeclarationInMemoryRepository outgoingDeclarationInMemoryRepository)
    {
        for (var i = 0; i < amount; i++)
        {
            var declaration = Any.OutgoingDeclaration();
            await outgoingDeclarationInMemoryRepository.Add(declaration);
        }
    }
}