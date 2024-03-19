using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using System.Threading;

namespace Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads
{
    public sealed class ApproveIncomingDeclarationUploadCommand : ICommand<ICommandResponse>
    {
        private ApproveIncomingDeclarationUploadCommand(IncomingDeclarationUploadId incomingDeclarationUploadId)
        {
            IncomingDeclarationUploadId = incomingDeclarationUploadId;
        }

        public IncomingDeclarationUploadId IncomingDeclarationUploadId { get; private set; }

        public static ApproveIncomingDeclarationUploadCommand Create(
            IncomingDeclarationUploadId incomingDeclarationUploadId)
        {
            return new ApproveIncomingDeclarationUploadCommand(incomingDeclarationUploadId);
        }
    }

    internal class ApproveIncomingDeclarationUploadCommandHandler : ICommandHandler<ApproveIncomingDeclarationUploadCommand,
            ICommandResponse>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly IUnitOfWork unitOfWork;

        public ApproveIncomingDeclarationUploadCommandHandler(IIncomingDeclarationRepository incomingDeclarationRepository, IUnitOfWork unitOfWork)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(ApproveIncomingDeclarationUploadCommand request,
            CancellationToken cancellationToken)
        {
            var incomingDeclarations =
                await incomingDeclarationRepository.GetTemporaryIncomingDeclarationsByUploadIdAsync(request.IncomingDeclarationUploadId, cancellationToken);

            foreach (var incomingDeclaration in incomingDeclarations)
            {
                incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.New);
                await incomingDeclarationRepository.Update(incomingDeclaration, cancellationToken);
            }

            await incomingDeclarationRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class ApproveIncomingDeclarationUploadCommandAuthorizer : IAuthorizer<ApproveIncomingDeclarationUploadCommand>
    {
        private readonly IExecutionContext executionContext;

        public ApproveIncomingDeclarationUploadCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(ApproveIncomingDeclarationUploadCommand instance,
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