using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.UpdateIncomingDeclaration
{
    public class UpdateIncomingDeclarationTest
    {
        [Fact]
        public async Task Should_Update_Incoming_Declaration()
        {
            //Arrange
            var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var declarationToUpdate = Any.IncomingDeclaration();
            await incomingDeclarationInMemoryRepository.Add(declarationToUpdate);

            var declarationUpdateValues = Any.IncomingDeclarationUpdateParameters();

            var query = UpdateIncomingDeclarationCommand.Create(declarationToUpdate.IncomingDeclarationId.Value,
                declarationUpdateValues);

            var handler =
                new UpdateIncomingDeclarationCommandHandler(incomingDeclarationInMemoryRepository, unitOfWork);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Company.Should().Be(declarationUpdateValues.Company.Value);
            result.Country.Should().Be(declarationUpdateValues.Country.Value);
            result.Product.Should().Be(declarationUpdateValues.Product.Value);
            result.Supplier.Should().Be(declarationUpdateValues.Supplier.Value);
            result.PosNumber.Should().Be(declarationUpdateValues.PosNumber.Value);
            result.RawMaterial.Should().Be(declarationUpdateValues.RawMaterial.Value);
            result.CountryOfOrigin.Should().Be(declarationUpdateValues.CountryOfOrigin.Value);
        }
    }
}