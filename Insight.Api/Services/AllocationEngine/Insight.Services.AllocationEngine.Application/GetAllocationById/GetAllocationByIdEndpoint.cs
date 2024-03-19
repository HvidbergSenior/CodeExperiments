using Insight.BuildingBlocks.Application.Queries;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.GetAllocationById
{
    public static class GetAllocationByIdEndpoint
    {
        public static void MapGetAllocationByIdEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    AllocationEngineEndpointUrls.GET_ALLOCATION_BY_ID_ENDPOINT,
                    async (
                        Guid id,
                        bool isOrderDescending,
                        string orderByProperty,
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var sortingParameters = SortingParameters.Create(isOrderDescending: isOrderDescending, orderByProperty: orderByProperty);

                        var query = GetAllocationByIdQuery.Create(AllocationId.Create(id), sortingParameters);

                        var results = await queryBus
                            .Send<GetAllocationByIdQuery,
                                GetAllocationByIdResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetAllocationByIdResponse>()
                .WithName("GetAllocationById")
                .WithTags("Allocations");
        }
    }
}