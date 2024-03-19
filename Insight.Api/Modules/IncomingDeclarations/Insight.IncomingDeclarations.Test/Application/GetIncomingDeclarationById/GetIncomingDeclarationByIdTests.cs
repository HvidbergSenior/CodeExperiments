using FluentAssertions;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationById;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.GetIncomingDeclarationById;

public class GetIncomingDeclarationByIdTests
{
    [Fact]
    public async Task Should_get_incoming_declaration()
    {
        //Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();

        var declaration = Any.IncomingDeclaration();
        await incomingDeclarationInMemoryRepository.Add(declaration);

        var query = GetIncomingDeclarationByIdQuery.Create(declaration.IncomingDeclarationId);

        var handler = new GetIncomingDeclarationByIdQueryHandler(incomingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Company.Should().Be(declaration.Company.Value);
    }
}