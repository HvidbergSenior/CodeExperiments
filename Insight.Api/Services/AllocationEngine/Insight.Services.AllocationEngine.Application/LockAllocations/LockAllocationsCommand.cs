using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.AllocationEngine.Domain;

namespace Insight.Services.AllocationEngine.Application.LockAllocations
{
    public sealed class LockAllocationsCommand : ICommand<ICommandResponse>
    {
        internal static LockAllocationsCommand Create()
        {
            return new LockAllocationsCommand();
        }
    }

    internal class LockAllocationsCommandHandler : ICommandHandler<LockAllocationsCommand, ICommandResponse>
    {
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IUnitOfWork unitOfWork;

        public LockAllocationsCommandHandler(IAllocationDraftRepository allocationDraftRepository, IUnitOfWork unitOfWork)
        {
            this.allocationDraftRepository = allocationDraftRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(LockAllocationsCommand request, CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.GetById(AllocationDraftId.Instance.Value, cancellationToken);   
            if (allocationDraft == null)
            {
                throw new NotFoundException("Allocation Draft not found");
            }

            allocationDraft.Lock();
            await allocationDraftRepository.Update(allocationDraft, cancellationToken);
            await allocationDraftRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }
    }

    internal class LockAllocationsCommandAuthorizer : IAuthorizer<LockAllocationsCommand>
    {
        private readonly IExecutionContext executionContext;

        public LockAllocationsCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(LockAllocationsCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
