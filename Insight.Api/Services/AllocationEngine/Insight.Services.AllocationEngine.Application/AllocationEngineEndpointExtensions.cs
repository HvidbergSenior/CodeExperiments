using Insight.Services.AllocationEngine.Application.ClearDraft;
using Insight.Services.AllocationEngine.Application.GetAllocationById;
using Insight.Services.AllocationEngine.Application.GetAllocations;
using Insight.Services.AllocationEngine.Application.GetAllocationSuggestions;
using Insight.Services.AllocationEngine.Application.LockAllocations;
using Insight.Services.AllocationEngine.Application.PostAutomaticAllocation;
using Insight.Services.AllocationEngine.Application.PostManualAllocation;
using Insight.Services.AllocationEngine.Application.PublishAllocations;
using Insight.Services.AllocationEngine.Application.UnlockAllocations;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application
{
    public static class AllocationEngineEndpointExtensions
    {
        public static IEndpointRouteBuilder MapAllocationEndpoints(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapGetAllocationSuggestionsEndpoint();
            endpoint.MapPostManualAllocationEndpoint();
            endpoint.MapPostAutomaticAllocationEndpoint();
            endpoint.MapGetAllocationsEndpoint();
            endpoint.MapLockAllocationsEndpoint();
            endpoint.MapUnlockAllocationsEndpoint();
            endpoint.MapPublishAllocationsEndpoint();
            endpoint.MapClearAllocationsEndpoint();
            endpoint.MapGetAllocationByIdEndpoint();
            return endpoint;
        }
    }
}