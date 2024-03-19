using FluentAssertions;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId;
using Insight.OutgoingDeclarations.Infrastructure;

namespace Insight.OutgoingDeclarations.Test.GetOutgoingDeclarationsByCustomerId;

public class GetOutgoingDeclarationsByCustomerIdTests
{
    [Fact]
    public async Task Should_get_outgoing_declarations()
    {
        //Arrange
        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var declaration = Any.OutgoingDeclaration();
        await outgoingDeclarationInMemoryRepository.Add(declaration);
        var declaration2 = Any.OutgoingDeclaration();
        await outgoingDeclarationInMemoryRepository.Add(declaration2);
        
        var query = GetOutgoingDeclarationsByCustomerIdQuery.Create(declaration.CustomerDetails.CustomerNumber);

        var handler = new GetOutgoingDeclarationsByCustomerIdQueryHandler(outgoingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        
        result.OutgoingDeclarationByCustomerIdResponse.Count().Should().Be(1);
        result.OutgoingDeclarationByCustomerIdResponse.Single().CustomerName.Should().Be(declaration.CustomerDetails.CustomerName.Value);
        result.OutgoingDeclarationByCustomerIdResponse.Single().CustomerNumber.Should().Be(declaration.CustomerDetails.CustomerNumber.Value);
    }
}