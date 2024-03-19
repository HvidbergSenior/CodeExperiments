using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.Services.AllocationEngine.Domain;
using Insight.Services.AllocationEngine.Service;

namespace Insight.Services.AllocationEngine.Application.PostManualAllocation
{
    public sealed class PostManualAllocationCommand : ICommand<ICommandResponse>
    {
        public ManualAllocationAssignments ManualAlloation { get; private set; }
        
        private PostManualAllocationCommand(ManualAllocationAssignments manualAlloation)
        {
            ManualAlloation = manualAlloation;
        }

        internal static PostManualAllocationCommand Create(ManualAllocationAssignments manualAlloation)
        {
            return new PostManualAllocationCommand(manualAlloation);
        }
    }

    internal class PostManualAllocationCommandHandler : ICommandHandler<PostManualAllocationCommand, ICommandResponse>
    {
        private readonly AllocationService allocationService;

        public PostManualAllocationCommandHandler(AllocationService allocationService)
        {
            this.allocationService = allocationService;
        }

        public async Task<ICommandResponse> Handle(PostManualAllocationCommand request, CancellationToken cancellationToken)
        {
            await allocationService.ManuallyAllocate(request.ManualAlloation, cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class PostManualAllocationCommandAuthorizer : IAuthorizer<PostManualAllocationCommand>
    {
        private readonly IExecutionContext executionContext;

        public PostManualAllocationCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(PostManualAllocationCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
