using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Stock;

namespace Insight.FuelTransactions.Application.GetStockTransactions
{
    public sealed class GetStockTransactionsQuery : IQuery<GetStockTransactionsQueryResponse>
    {
        public PaginationParameters PaginationParameters { get; private set; }
        public SortingParameters SortingParameters { get; private set; }
        public StockFilteringParameters StockFilteringParameters { get; private set; }

        private GetStockTransactionsQuery(PaginationParameters paginationParameters,
            SortingParameters sortingParameters, StockFilteringParameters stockFilteringParameters)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            StockFilteringParameters = stockFilteringParameters;
        }

        public static GetStockTransactionsQuery Create(PaginationParameters paginationParameters,
            SortingParameters sortingParameters, StockFilteringParameters stockFilteringParameters)
        {
            return new GetStockTransactionsQuery(paginationParameters, sortingParameters, stockFilteringParameters);
        }
    }

    internal class GetStockTransactionsQueryHandler : IQueryHandler<GetStockTransactionsQuery, GetStockTransactionsQueryResponse>
    {
        private readonly IStockTransactionsRepository stockTransactionsRepository;

        public GetStockTransactionsQueryHandler(IStockTransactionsRepository stockTransactionsRepository)
        {
            this.stockTransactionsRepository = stockTransactionsRepository;
        }

        public async Task<GetStockTransactionsQueryResponse> Handle(GetStockTransactionsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await stockTransactionsRepository.GetStockTransactionsAsync(request.PaginationParameters.Page,
                                                                                                                 request.PaginationParameters.PageSize,
                                                                                                                 request.SortingParameters,
                                                                                                                 request.StockFilteringParameters,
                                                                                                                 cancellationToken);

            var result = items.Select(c => new StockTransactionResponse(c.Id, c.CompanyId.Value,
                                                                               c.CompanyName.Value,
                                                                               c.ProductNumber.Value,
                                                                               c.ProductName.Value,
                                                                               c.Quantity.Value,
                                                                               c.Country.Value,
                                                                               c.Location.Value,
                                                                               c.LocationId,
                                                                               c.Allocations.TotalAllocatedVolume,
                                                                               GetAllocationPercentage(c.Allocations.TotalAllocatedVolume, c.Quantity.Value),
                                                                               c.RemainingVolume));

            return new GetStockTransactionsQueryResponse(result.ToList().AsReadOnly(),request.PaginationParameters.Page * request.PaginationParameters.PageSize < totalCount, totalCount);
        }

        private static decimal GetAllocationPercentage(decimal allocatedQuantity, decimal totalQuantity)
        {
            if (totalQuantity == 0)
            {
                return 0;
            }
            return allocatedQuantity / totalQuantity * 100;
        }
    }


    internal class GetStockTransactionsQueryAuthorizer : IAuthorizer<GetStockTransactionsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetStockTransactionsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetStockTransactionsQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
