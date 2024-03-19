using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.AllocationEngine.Domain;

namespace Insight.Services.AllocationEngine.Application.UnlockAllocations
{
    public sealed class UnlockAllocationsCommand : ICommand<ICommandResponse>
    {
        internal static UnlockAllocationsCommand Create()
        {
            return new UnlockAllocationsCommand();
        }
    }

    internal class UnlockAllocationsCommandHandler : ICommandHandler<UnlockAllocationsCommand, ICommandResponse>
    {
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IUnitOfWork unitOfWork;

        public UnlockAllocationsCommandHandler(IAllocationDraftRepository allocationDraftRepository, IUnitOfWork unitOfWork)
        {
            this.allocationDraftRepository = allocationDraftRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(UnlockAllocationsCommand request, CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.GetById(AllocationDraftId.Instance.Value, cancellationToken);   
            if (allocationDraft == null)
            {
                throw new NotFoundException("Allocation Draft not found");
            }

            allocationDraft.Unlock();
            await allocationDraftRepository.Update(allocationDraft, cancellationToken);
            await allocationDraftRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }
    }

    internal class UnlockAllocationsCommandAuthorizer : IAuthorizer<UnlockAllocationsCommand>
    {
        private readonly IExecutionContext executionContext;

        public UnlockAllocationsCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UnlockAllocationsCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
