using Insight.BuildingBlocks.Application.Queries;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize
{
    public static class GetIncomingDeclarationsByPageAndPageSizeEndpoint
    {
        public static void MapGetIncomingDeclarationsByPageAndPageSizeEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    IncomingDeclarationsEndpointUrls
                        .GET_INCOMING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT,
                    async (
                        DateOnly? dateFrom,
                        DateOnly? dateTo,
                        string? product,
                        string? company,
                        string? supplier,
                        int page,
                        int pageSize,
                        bool isOrderDescending,
                        string orderByProperty,
                        IQueryBus queryBus, 
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var productFilter = !string.IsNullOrEmpty(product) ? Product.Create(product) : Product.Empty();
                        var companyFilter = !string.IsNullOrEmpty(company) ? Company.Create(company) : Company.Empty();
                        var supplierFilter = !string.IsNullOrEmpty(supplier) ? Supplier.Create(supplier) : Supplier.Empty();
                        var datePeriod = DatePeriod.Empty();
                        
                        if (dateFrom is not null && dateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)dateFrom, (DateOnly)dateTo);
                        }
                        
                        var filteringParameters = FilteringParameters.Create(datePeriod, productFilter, companyFilter, supplierFilter);

                        var paginationParameters = PaginationParameters.Create(page, pageSize);
                        var sortingParameters = SortingParameters.Create(isOrderDescending: isOrderDescending, orderByProperty: orderByProperty);
                        var query = GetIncomingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters, sortingParameters, filteringParameters, new List<IncomingDeclarationState>() { IncomingDeclarationState.New });

                        var results = await queryBus
                            .Send<GetIncomingDeclarationsByPageAndPageSizeQuery,
                                GetIncomingDeclarationsByPageAndPageSizeResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetIncomingDeclarationsByPageAndPageSizeResponse>()
                .WithName("GetIncomingDeclarationsByPageAndPageSize")
                .WithTags("IncomingDeclarations");
        }
    }
}