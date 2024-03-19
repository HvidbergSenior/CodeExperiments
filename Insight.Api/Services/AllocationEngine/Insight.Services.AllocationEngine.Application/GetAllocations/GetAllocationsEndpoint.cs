using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PaginationParameters = Insight.OutgoingDeclarations.Domain.PaginationParameters;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.Services.AllocationEngine.Application.GetAllocations
{
    public static class GetAllocationsEndpoint
    {
        public static void MapGetAllocationsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    AllocationEngineEndpointUrls
                        .GET_ALLOCATIONS_ENDPOINT,
                    async (
                        DateOnly? dateFrom,
                        DateOnly? dateTo,
                        string? product,
                        string? company,
                        string? customer,
                        int page,
                        int pageSize,
                        bool isOrderDescending,
                        string orderByProperty,
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        Product productFilter = !string.IsNullOrEmpty(product) ? Product.Create(product) : Product.Empty();
                        Company companyFilter = !string.IsNullOrEmpty(company) ? Company.Create(company) : Company.Empty();
                        CustomerName customerFilter = !string.IsNullOrEmpty(customer) ? CustomerName.Create(customer) : CustomerName.Empty();
                        DatePeriod datePeriod = DatePeriod.Empty();
                        
                        if (dateFrom is not null && dateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)dateFrom, (DateOnly)dateTo);
                        }
                        var filteringParameters = FilteringParameters.Create(datePeriod, productFilter, companyFilter, customerFilter);
                        var sortingParameters = SortingParameters.Create(isOrderDescending, orderByProperty);
                        var paginationParameters = PaginationParameters.Create(page, pageSize);
                        
                        var query = GetAllocationsQuery.Create(filteringParameters, sortingParameters, paginationParameters);

                        var result = await queryBus.Send<GetAllocationsQuery, GetAllocationsResponse>(query, cancellationToken);

                        return Results.Ok(result);
                    })
                .RequireAuthorization()
                .Produces<GetAllocationsResponse>()
                .WithName("GetAllocations")
                .WithTags("Allocations");
        }
    }
}
