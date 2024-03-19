using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PaginationParameters = Insight.FuelTransactions.Domain.PaginationParameters;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Application.GetOutgoingFuelTransactions
{
    public static class GetOutgoingFuelTransactionsEndpoint
    {
        public static void MapGetOutgoingFuelTransactionsEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapGet(
                    OutgoingFuelTransactionsEndpointUrls.GET_OUTGOING_FUEL_TRANSACTIONS_ENDPOINT,
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
                        var productName = !string.IsNullOrEmpty(product) ? ProductName.Create(product) : ProductName.Empty();
                        var companyName = !string.IsNullOrEmpty(company) ? CompanyName.Create(company) : CompanyName.Empty();
                        var customerName = !string.IsNullOrEmpty(customer) ? CustomerName.Create(customer) : CustomerName.Empty();

                        var datePeriod = DatePeriod.Empty();

                        if (dateFrom is not null && dateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)dateFrom, (DateOnly)dateTo);
                        }

                        var paginationParameters = PaginationParameters.Create(page, pageSize);
                        var sortingParameters = SortingParameters.Create(isOrderDescending: isOrderDescending, orderByProperty: orderByProperty);
                        var filteringParameters = FilteringParameters.Create(datePeriod, productName, customerName, companyName);

                        var query = GetOutgoingFuelTransactionsQuery.Create(paginationParameters, sortingParameters, filteringParameters);
                        var results = await queryBus
                            .Send<GetOutgoingFuelTransactionsQuery,
                                GetOutgoingFuelTransactionsQueryResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .Produces<GetOutgoingFuelTransactionsQueryResponse>()
                .RequireAuthorization()
                .WithName("GetOutgoingFuelTransactions")
                .WithTags("OutgoingFuelTransactions");
        }
    }
}
