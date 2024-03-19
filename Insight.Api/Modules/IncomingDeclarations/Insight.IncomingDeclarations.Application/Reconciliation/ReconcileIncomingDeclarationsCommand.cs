using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.Reconciliation
{
    public sealed class ReconcileIncomingDeclarationsCommand : ICommand<ICommandResponse>
    {
        private ReconcileIncomingDeclarationsCommand(IncomingDeclarationId[] incomingDeclarationIds)
        {
            IncomingDeclarationIds = incomingDeclarationIds;
        }

        public IncomingDeclarationId[] IncomingDeclarationIds { get; private set; }

        public static ReconcileIncomingDeclarationsCommand Create(IncomingDeclarationId[] incomingDeclarationIds)
        {
            return new ReconcileIncomingDeclarationsCommand(incomingDeclarationIds);
        }
    }

    internal class
        ReconcileIncomingDeclarationsCommandHandler : ICommandHandler<ReconcileIncomingDeclarationsCommand,
            ICommandResponse>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly IUnitOfWork unitOfWork;

        public ReconcileIncomingDeclarationsCommandHandler(IIncomingDeclarationRepository incomingDeclarationRepository,
            IUnitOfWork unitOfWork)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(ReconcileIncomingDeclarationsCommand request,
            CancellationToken cancellationToken)
        {
            var incomingDeclarations = await incomingDeclarationRepository.GetByIncomingDeclarationIdsAndStateAsync(request.IncomingDeclarationIds, IncomingDeclarationState.New, cancellationToken);

            foreach (var incomingDeclaration in incomingDeclarations)
            {
                incomingDeclaration.SetIncomingDeclarationState(IncomingDeclarationState.Reconciled);
                await incomingDeclarationRepository.Update(incomingDeclaration, cancellationToken);
            }

            await incomingDeclarationRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class ReconcileIncomingDeclarationsCommandAuthorizer : IAuthorizer<ReconcileIncomingDeclarationsCommand>
    {
        private readonly IExecutionContext executionContext;

        public ReconcileIncomingDeclarationsCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(ReconcileIncomingDeclarationsCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}