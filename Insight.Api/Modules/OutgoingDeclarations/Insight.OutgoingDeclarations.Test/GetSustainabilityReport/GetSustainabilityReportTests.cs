using FluentAssertions;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId;
using Insight.OutgoingDeclarations.Infrastructure;

namespace Insight.OutgoingDeclarations.Test.GetSustainabilityReport;

public class GetSustainabilityReportTests
{
    [Fact]
    public async Task Should_get_sustainability_report()
    {
        //Arrange
        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var declaration = Any.OutgoingDeclaration();
        await outgoingDeclarationInMemoryRepository.Add(declaration);

        var query = GetOutgoingDeclarationsByCustomerIdQuery.Create(declaration.CustomerDetails.CustomerNumber);

        var handler = new GetOutgoingDeclarationsByCustomerIdQueryHandler(outgoingDeclarationInMemoryRepository);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        // result.OutgoingDeclarationByIdResponse..Should().Be(declaration.Company.Value);
    }
}