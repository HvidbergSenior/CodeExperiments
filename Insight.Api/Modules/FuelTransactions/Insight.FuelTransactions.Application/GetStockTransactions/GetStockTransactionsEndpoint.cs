using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaginationParameters = Insight.FuelTransactions.Domain.PaginationParameters;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Application.GetStockTransactions
{
    public static class GetStockTransactionsEndpoint
    {
        public static void MapGetStockTransactionsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    StockTransactionsEndpointUrls.GET_STOCK_TRANSACTIONS_ENDPOINT,
                    async (
                        DateOnly? dateFrom,
                        DateOnly? dateTo,
                        string? product,
                        string? company,
                        int page,
                        int pageSize,
                        bool isOrderDescending,
                        string orderByProperty,                        
                        CancellationToken cancellationToken,
                        IQueryBus queryBus
                    ) =>
                    {
                        var productName = !string.IsNullOrEmpty(product) ? ProductName.Create(product) : ProductName.Empty();
                        var companyName = !string.IsNullOrEmpty(company) ? CompanyName.Create(company) : CompanyName.Empty();
                        
                        var datePeriod = DatePeriod.Empty();

                        if (dateFrom is not null && dateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)dateFrom, (DateOnly)dateTo);
                        }

                        var paginationParameters = PaginationParameters.Create(page, pageSize);
                        var sortingParameters = SortingParameters.Create(isOrderDescending: isOrderDescending, orderByProperty: orderByProperty);
                        var filteringParameters = StockFilteringParameters.Create(datePeriod, productName, companyName);

                        var query = GetStockTransactionsQuery.Create(paginationParameters, sortingParameters, filteringParameters);
                        var results = await queryBus
                            .Send<GetStockTransactionsQuery,
                                GetStockTransactionsQueryResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .Produces<GetStockTransactionsQueryResponse>()
                .RequireAuthorization()
                .WithName("GetStockTransactions")
                .WithTags("Stocks");
        }
    }
}
