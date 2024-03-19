using Insight.BuildingBlocks.Application.Queries;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.Services.AllocationEngine.Application.GetAllocationSuggestions
{
    public static class GetAllocationSuggestionsEndpoint
    {
        public static void MapGetAllocationSuggestionsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    AllocationEngineEndpointUrls
                        .GET_ALLOCATION_SUGGESTIONS_ENDPOINT,
                    async (
                      Guid customerId,
                      DateOnly? startDate,
                      DateOnly? endDate,
                      string product,
                      string country,
                      string location,
                      int page,
                      int pageSize,
                      bool isOrderDescending,
                      string orderByProperty,
                      IQueryBus queryBus,
                      CancellationToken cancellationToken
                    ) =>
                    {
                        var fuelTransactionCustomerId = FuelTransactionCustomerId.Create(customerId);

                        var filterStartDate = startDate ?? DateOnly.MinValue;
                        var filterEndDate = endDate ?? DateOnly.MaxValue;

                        var suggestionRequest = SuggestionRequest.Create(fuelTransactionCustomerId,
                                                                      filterStartDate,
                                                                      filterEndDate,
                                                                      Product.Create(product),
                                                                      Country.Create(country),
                                                                      PlaceOfDispatch.Create(location), isOrderDescending, OrderByProperty.Create(orderByProperty));

                        var query = GetAllocationSuggestionsQuery.Create(suggestionRequest, page, pageSize, isOrderDescending, orderByProperty);

                        var results = await queryBus.Send<GetAllocationSuggestionsQuery, GetAllocationSuggestionsResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetAllocationSuggestionsResponse>()
                .WithName("GetAllocationSuggestions")
                .WithTags("Allocations");
        }
    }
}
