using Insight.BuildingBlocks.Application.Commands;
using Insight.Services.AllocationEngine.Domain;
using Insight.Services.AllocationEngine.Service;

namespace Insight.Services.AllocationEngine.Application.PostAutomaticAllocation
{
    public sealed class PostAutomaticAllocationCommand : ICommand<ICommandResponse>
    {
        public AutoAllocationFilteringParameters AutoAllocationFilteringParameters { get; private set; }

        private PostAutomaticAllocationCommand(AutoAllocationFilteringParameters autoAllocationFilteringParameters)
        {
            AutoAllocationFilteringParameters = autoAllocationFilteringParameters;
        }
      

        internal static PostAutomaticAllocationCommand Create(AutoAllocationFilteringParameters autoAllocationFilteringParameters)
        {
            return new PostAutomaticAllocationCommand(autoAllocationFilteringParameters);
        }
    }

    internal class PostAutomaticAllocationCommandHandler : ICommandHandler<PostAutomaticAllocationCommand, ICommandResponse>
    {
        private readonly AllocationService allocationService;

        public PostAutomaticAllocationCommandHandler(AllocationService allocationService)
        {
            this.allocationService = allocationService;
        }

        public async Task<ICommandResponse> Handle(PostAutomaticAllocationCommand request, CancellationToken cancellationToken)
        {
            await allocationService.AutoAllocate(request.AutoAllocationFilteringParameters.StartDate, request.AutoAllocationFilteringParameters.EndDate, request.AutoAllocationFilteringParameters.FilterProductName, request.AutoAllocationFilteringParameters.FilterCompanyName, request.AutoAllocationFilteringParameters.FilterCustomerName, cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

}
