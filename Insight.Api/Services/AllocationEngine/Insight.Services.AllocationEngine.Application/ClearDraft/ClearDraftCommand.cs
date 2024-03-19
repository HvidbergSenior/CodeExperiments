using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.AllocationEngine.Service;

namespace Insight.Services.AllocationEngine.Application.ClearDraft
{
    public sealed class ClearDraftCommand : ICommand<ICommandResponse>
    {
        internal static ClearDraftCommand Create()
        {
            return new ClearDraftCommand();
        }
    }

    internal class ClearDraftCommandHandler : ICommandHandler<ClearDraftCommand, ICommandResponse>
    {
        private readonly AllocationService allocationService;
        private readonly IUnitOfWork unitOfWork;

        public ClearDraftCommandHandler(AllocationService allocationService, IUnitOfWork unitOfWork)
        {
            this.allocationService = allocationService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(ClearDraftCommand request, CancellationToken cancellationToken)
        {
            await allocationService.ClearDraftAsync(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }
    }

    internal class ClearDraftCommandAuthorizer : IAuthorizer<ClearDraftCommand>
    {
        private readonly IExecutionContext executionContext;

        public ClearDraftCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(ClearDraftCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
