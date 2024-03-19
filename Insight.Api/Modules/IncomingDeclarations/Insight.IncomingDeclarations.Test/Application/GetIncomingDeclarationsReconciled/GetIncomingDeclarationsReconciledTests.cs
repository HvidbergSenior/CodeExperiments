using FluentAssertions;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.GetIncomingDeclarationsReconciled;

public class GetIncomingDeclarationsReconciledTests
{
    public static IEnumerable<object[]> QueryParametersData()
    {
        yield return new object[] { new List<IncomingDeclarationState>(), 3, 7, 10 };
        yield return new object[] { new List<IncomingDeclarationState> { IncomingDeclarationState.New }, 3, 7, 3 };
        yield return new object[] { new List<IncomingDeclarationState> { IncomingDeclarationState.Reconciled }, 3, 7, 7 };
        yield return new object[] { new List<IncomingDeclarationState> { IncomingDeclarationState.Temporary, IncomingDeclarationState.Allocated }, 3, 7, 0 };
        yield return new object[] { new List<IncomingDeclarationState> { IncomingDeclarationState.Temporary, IncomingDeclarationState.Reconciled }, 3, 7, 7 };
    }

    [Theory]
    [MemberData(nameof(QueryParametersData))]
    public async Task ShouldGetReconciledDeclarations(List<IncomingDeclarationState> states, int amountOfDeclarationsNew, int amountOfDeclarationsReconciled, int expectedTotalAmount
    )
    {
        // Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
        CreateIncomingDeclarations(amountOfDeclarationsNew, IncomingDeclarationState.New,
            incomingDeclarationInMemoryRepository);
        CreateIncomingDeclarations(amountOfDeclarationsReconciled, IncomingDeclarationState.Reconciled,
            incomingDeclarationInMemoryRepository);
        var amountOfDeclarations = amountOfDeclarationsNew + amountOfDeclarationsReconciled;
        var paginationParameters = PaginationParameters.Create(0, amountOfDeclarations);
        var sortingParameters = SortingParameters.Create(true, "Company");
        var filterParameters = FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), Supplier.Empty());

        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, sortingParameters, filterParameters, states);
        var handler = new GetIncomingDeclarationsByPageAndPageSizeQueryHandler(incomingDeclarationInMemoryRepository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(expectedTotalAmount);
    }

    private static async void CreateIncomingDeclarations(int amount,
        IncomingDeclarationState incomingDeclarationState,
        IncomingDeclarationInMemoryRepository incomingDeclarationInMemoryRepository
    )
    {
        for (var i = 0; i < amount; i++)
        {
            var declaration = Any.IncomingDeclaration();
            declaration.SetIncomingDeclarationState(incomingDeclarationState);
            await incomingDeclarationInMemoryRepository.Add(declaration);
        }
    }
}