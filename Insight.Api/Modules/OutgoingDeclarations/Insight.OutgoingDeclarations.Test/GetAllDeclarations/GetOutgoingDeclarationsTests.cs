using FluentAssertions;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations;
using Insight.OutgoingDeclarations.Infrastructure;
namespace Insight.OutgoingDeclarations.Test.GetAllDeclarations;

public class GetOutgoingDeclarationsTests
{
    [Fact]
    public async Task Should_get_outgoing_declarations()
    {
        //Arrange
        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var declaration = Any.OutgoingDeclaration();
        await outgoingDeclarationInMemoryRepository.Add(declaration);

        var query = GetOutgoingDeclarationsQuery.Create();

        var handler = new GetOutgoingDeclarationsQueryHandler(outgoingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull(); 
        result.OutgoingDeclarationsResponses.First().CustomerNumber.Should().Be(declaration.CustomerDetails.CustomerNumber.Value);
        result.OutgoingDeclarationsResponses.First().CustomerName.Should().Be(declaration.CustomerDetails.CustomerName.Value);
    }
}