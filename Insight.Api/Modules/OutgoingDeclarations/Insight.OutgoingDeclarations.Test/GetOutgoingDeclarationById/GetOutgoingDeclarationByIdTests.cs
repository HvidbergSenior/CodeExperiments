using FluentAssertions;
using Insight.BuildingBlocks.Application.Queries;
using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Infrastructure;
using Moq;

namespace Insight.OutgoingDeclarations.Test.GetOutgoingDeclarationById;

public class GetOutgoingDeclarationByIdTests
{
    [Fact]
    public async Task Should_get_outgoing_declaration()
    {
        //Arrange
        var queryBusMock = new Mock<IQueryBus>();
        queryBusMock.Setup(c => c.Send<GetIncomingDeclarationsByIdsQuery, GetIncomingDeclarationsDto>(It.IsAny<GetIncomingDeclarationsByIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => new GetIncomingDeclarationsDto(new List<GetIncomingDeclarationDto>(){ new GetIncomingDeclarationDto(Guid.NewGuid(),"","","","","","","","", DateOnly.MaxValue, 100,100, 33333333, DateOnly.MaxValue, "", "", "", "", true, true, true, "", true, "", 145, true, true,true,"", 2, 4,6,7,4,5,6, 9, 2, 4,7)}));

        var outgoingDeclarationInMemoryRepository = new OutgoingDeclarationInMemoryRepository();

        var declaration = Any.OutgoingDeclaration();
        await outgoingDeclarationInMemoryRepository.Add(declaration);

        var query = GetOutgoingDeclarationByIdQuery.Create(declaration.OutgoingDeclarationId);

        var handler = new GetOutgoingDeclarationByIdQueryHandler(outgoingDeclarationInMemoryRepository, queryBusMock.Object);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull(); 
        result.OutgoingDeclarationByIdResponse.CustomerNumber.Should().Be(declaration.CustomerDetails.CustomerNumber.Value);
        result.OutgoingDeclarationByIdResponse.CustomerName.Should().Be(declaration.CustomerDetails.CustomerName.Value);
    }
}