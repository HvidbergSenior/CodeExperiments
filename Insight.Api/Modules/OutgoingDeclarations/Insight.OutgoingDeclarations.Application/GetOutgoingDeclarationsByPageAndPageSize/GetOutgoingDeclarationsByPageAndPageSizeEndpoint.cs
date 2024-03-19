using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PaginationParameters = Insight.OutgoingDeclarations.Domain.PaginationParameters;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize
{
    public static class GetOutgoingDeclarationsByPageAndPageSizeEndpoint
    {
        public static void MapGetOutgoingDeclarationsByPageAndPageSizeEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    OutgoingDeclarationsEndpointUrls
                        .GET_OUTGOING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT,
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
                        var productFilter = !string.IsNullOrEmpty(product) ? Product.Create(product) : Product.Empty();
                        var companyFilter = !string.IsNullOrEmpty(company) ? Company.Create(company) : Company.Empty();
                        var customerNameFilter = !string.IsNullOrEmpty(customer) ? CustomerName.Create(customer) : CustomerName.Empty();
                        var datePeriod = DatePeriod.Empty();
                        
                        if (dateFrom is not null && dateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)dateFrom, (DateOnly)dateTo);
                        }
                        
                        var filteringParameters = FilteringParameters.Create(datePeriod, productFilter, companyFilter, customerNameFilter);
                        var paginationParameters = PaginationParameters.Create(page, pageSize);
                        var sortingParameters = SortingParameters.Create(isOrderDescending: isOrderDescending,
                            orderByProperty: orderByProperty);
                        var query = GetOutgoingDeclarationsByPageAndPageSizeQuery.Create(paginationParameters,
                            sortingParameters, filteringParameters);

                        var results = await queryBus
                            .Send<GetOutgoingDeclarationsByPageAndPageSizeQuery,
                                GetOutgoingDeclarationsByPageAndPageSizeResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetOutgoingDeclarationsByPageAndPageSizeResponse>()
                .WithName("GetOutgoingDeclarationsByPageAndPageSize")
                .WithTags("OutgoingDeclarations");
        }
    }
}