using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.CancelIncomingDeclarationsByUploadId;

public class CancelIncomingDeclarationsByUploadIdTests
{
     [Fact]
     public async Task Should_cancel_incoming_declarations()
     {
         //Arrange
         var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
         var unitOfWork = new FakeUnitOfWork();
         var declaration1 = Any.IncomingDeclaration();
         var declaration2 = Any.IncomingDeclaration();

         await incomingDeclarationInMemoryRepository.Add(declaration1);
         await incomingDeclarationInMemoryRepository.Add(declaration2);
         
         var declarationsBefore = incomingDeclarationInMemoryRepository.Query().ToList();

         var query = CancelIncomingDeclarationsByUploadIdCommand.Create(declaration1.IncomingDeclarationUploadId);
         var handler = new CancelIncomingDeclarationsByUploadIdCommandHandler(incomingDeclarationInMemoryRepository, unitOfWork);
         
         //Act
         var result = await handler.Handle(query, CancellationToken.None);
         var declarationsAfterCancel = incomingDeclarationInMemoryRepository.Query().ToList();
         
         //Assert
         //TODO: The CancelIncomingDeclarationsByUploadIdCommand intentionally deletes all Declarations with state temporary and not just the ones with the given UploadId
         //When a more specific declaration-temporary-state-clean-up tool is developed, this method will only delete those with the given uploadId (and the assertions will need to be changed)
         result.Should().NotBeNull();
         declarationsBefore.Count.Should().Be(2);
         declarationsAfterCancel.Count.Should().Be(0);
     }
}