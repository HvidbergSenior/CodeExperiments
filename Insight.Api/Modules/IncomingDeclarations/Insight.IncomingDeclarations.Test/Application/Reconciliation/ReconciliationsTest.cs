using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.IncomingDeclarations.Application.Reconciliation;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;

namespace Insight.IncomingDeclarations.Test.Application.Reconciliation
{
    public class ReconciliationsTest
    {
        [Fact]
        public async Task Should_Reconcile_Incoming_Declaration_Upload()
        {
            //Arrange
            var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();
            var declarationToReconcile = Any.IncomingDeclaration();

            declarationToReconcile.SetIncomingDeclarationState(IncomingDeclarationState.New);

            var randomDeclaration = Any.IncomingDeclaration();

            await incomingDeclarationInMemoryRepository.Add(declarationToReconcile);
            await incomingDeclarationInMemoryRepository.Add(randomDeclaration);

            var query = ReconcileIncomingDeclarationsCommand.Create(new IncomingDeclarationId[] { declarationToReconcile.IncomingDeclarationId });

            var handler = new ReconcileIncomingDeclarationsCommandHandler(incomingDeclarationInMemoryRepository, unitOfWork);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);
            var declarationsAfterReconciliation = incomingDeclarationInMemoryRepository.Query().ToList();

            //Assert
            result.Should().NotBeNull();
            declarationsAfterReconciliation.Count.Should().Be(2);
            declarationsAfterReconciliation[0].IncomingDeclarationState.Should().Be(IncomingDeclarationState.Reconciled);
            declarationsAfterReconciliation[1].IncomingDeclarationState.Should().Be(IncomingDeclarationState.Temporary);
        }
    }
}
