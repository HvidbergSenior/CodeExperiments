using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.ApproveIncomingDeclarationUploads;

public class ApproveIncomingDeclarationUploadsTests
{
     [Fact]
     public async Task Should_approve_incoming_declaration_upload()
     {
         //Arrange
         var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
         var unitOfWork = new FakeUnitOfWork();
         var declarationToApprove = Any.IncomingDeclaration();
         var randomDeclaration = Any.IncomingDeclaration();

         await incomingDeclarationInMemoryRepository.Add(declarationToApprove);
         await incomingDeclarationInMemoryRepository.Add(randomDeclaration);

         var query = ApproveIncomingDeclarationUploadCommand.Create(declarationToApprove.IncomingDeclarationUploadId);
    
         var handler = new ApproveIncomingDeclarationUploadCommandHandler(incomingDeclarationInMemoryRepository, unitOfWork);
         
         //Act
         var result = await handler.Handle(query, CancellationToken.None);
         var declarationsAfterApprove = incomingDeclarationInMemoryRepository.Query().ToList();
         
         //Assert
         result.Should().NotBeNull();
         declarationsAfterApprove.Count.Should().Be(2);
         declarationsAfterApprove[0].IncomingDeclarationState.Should().Be(IncomingDeclarationState.New);
         declarationsAfterApprove[1].IncomingDeclarationState.Should().Be(IncomingDeclarationState.Temporary);
     }

     private async void CreateIncomingDeclarations(int amount, IncomingDeclarationInMemoryRepository incomingDeclarationInMemoryRepository)
     {
         for (int i = 0; i < amount; i++)
         {
             await incomingDeclarationInMemoryRepository.Add(Any.IncomingDeclaration());
         }
     }
}