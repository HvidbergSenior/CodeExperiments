using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using System.Threading;

namespace Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId
{
    public sealed class CancelIncomingDeclarationsByUploadIdCommand : ICommand<ICommandResponse>
    {
        private CancelIncomingDeclarationsByUploadIdCommand(IncomingDeclarationUploadId incomingDeclarationUploadId)
        {
            IncomingDeclarationUploadId = incomingDeclarationUploadId;
        }

        public IncomingDeclarationUploadId IncomingDeclarationUploadId { get; private set; }

        public static CancelIncomingDeclarationsByUploadIdCommand Create(
            IncomingDeclarationUploadId incomingDeclarationUploadId)
        {
            return new CancelIncomingDeclarationsByUploadIdCommand(incomingDeclarationUploadId);
        }
    }

    internal class CancelIncomingDeclarationsByUploadIdCommandHandler : ICommandHandler<CancelIncomingDeclarationsByUploadIdCommand,
            ICommandResponse>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly IUnitOfWork unitOfWork;

        public CancelIncomingDeclarationsByUploadIdCommandHandler(IIncomingDeclarationRepository incomingDeclarationRepository, IUnitOfWork unitOfWork)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(CancelIncomingDeclarationsByUploadIdCommand request,
            CancellationToken cancellationToken)
        {
            //TODO: We intentionally calls the method, DeleteTemporaryIncomingDeclaration, instead of DeleteTemporaryIncomingDeclarationBYID. When a clean-up tool, which removes all declarations with state temporary is implemented, we change it.

            await incomingDeclarationRepository.DeleteTemporaryIncomingDeclarationsAsync(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class CancelIncomingDeclarationsByUploadIdCommandAuthorizer : IAuthorizer<CancelIncomingDeclarationsByUploadIdCommand>
    {
        private readonly IExecutionContext executionContext;

        public CancelIncomingDeclarationsByUploadIdCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }
        public async Task<AuthorizationResult> Authorize(CancelIncomingDeclarationsByUploadIdCommand instance,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}