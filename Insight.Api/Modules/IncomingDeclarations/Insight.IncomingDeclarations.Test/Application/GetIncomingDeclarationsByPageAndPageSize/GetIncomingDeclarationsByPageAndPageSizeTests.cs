using FluentAssertions;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.GetIncomingDeclarationsByPageAndPageSize;

public class GetIncomingDeclarationsByPageAndPageSizeTests
{
    [Fact]
    public async Task Should_get_incoming_declarations()
    {
        //Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
        const int totalAmountOfDeclarations = 10;
        //page is Skip
        const int page = 2;
        //pageSize is Take
        const int pageSize = 5;

        CreateIncomingDeclarations(totalAmountOfDeclarations, incomingDeclarationInMemoryRepository);

        var paginationParameters = PaginationParameters.Create(page, pageSize);
        var sortingParameters = SortingParameters.Create(true, "Company");
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), Supplier.Empty());

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, sortingParameters, filterParameters,
            new List<IncomingDeclarationState>());

        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        var remaining = GetDeclarationsToReceive(totalAmountOfDeclarations, page, pageSize);

        result.Should().NotBeNull();
        result.TotalAmountOfDeclarations.Should().Be(totalAmountOfDeclarations);
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(remaining);
        result.HasMoreDeclarations.Should().BeTrue();
    }

    private int GetDeclarationsToReceive(int totalAmountOfDeclarations, int page, int pageSize)
    {
        if (pageSize == 0)
        {
            return 0;
        }

        var skipped = (page - 1) * pageSize;

        var remainingAfterSkipped = totalAmountOfDeclarations - skipped;
        return remainingAfterSkipped > pageSize ? pageSize : remainingAfterSkipped;
    }

    private static async void CreateIncomingDeclarations(int amount,
        IncomingDeclarationInMemoryRepository incomingDeclarationInMemoryRepository)
    {
        for (var i = 0; i < amount; i++)
        {
            var declaration = Any.IncomingDeclaration();
            declaration.SetIncomingDeclarationState(IncomingDeclarationState.New);
            await incomingDeclarationInMemoryRepository.Add(declaration);
        }
    }
}