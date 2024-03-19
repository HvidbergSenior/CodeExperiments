using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;

namespace Insight.FuelTransactions.Application.GetOutgoingFuelTransactions
{
    public sealed class GetOutgoingFuelTransactionsQuery : IQuery<GetOutgoingFuelTransactionsQueryResponse>
    {
        public PaginationParameters PaginationParameters { get; private set; }
        public SortingParameters SortingParameters { get; private set; }
        public FilteringParameters FilteringParameters { get; private set; }

        private GetOutgoingFuelTransactionsQuery(PaginationParameters paginationParameters,
            SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            FilteringParameters = filteringParameters;
        }

        public static GetOutgoingFuelTransactionsQuery Create(PaginationParameters paginationParameters,
            SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            return new GetOutgoingFuelTransactionsQuery(paginationParameters, sortingParameters, filteringParameters);
        }
    }

    internal class GetOutgoingFuelTransactionsQueryHandler : IQueryHandler<GetOutgoingFuelTransactionsQuery, GetOutgoingFuelTransactionsQueryResponse>
    {
        private readonly IOutgoingFuelTransactionsRepository outgoingFuelTransactionsRepository;

        public GetOutgoingFuelTransactionsQueryHandler(IOutgoingFuelTransactionsRepository outgoingFuelTransactionsRepository)
        {
            this.outgoingFuelTransactionsRepository = outgoingFuelTransactionsRepository;
        }

        public async Task<GetOutgoingFuelTransactionsQueryResponse> Handle(GetOutgoingFuelTransactionsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount, totalQuantity) = await outgoingFuelTransactionsRepository.GetByAggregatedFuelTransactionsAsync(request.PaginationParameters.Page, request.PaginationParameters.PageSize, request.SortingParameters, request.FilteringParameters, cancellationToken);

            var result = items.Select(c => new OutgoingFuelTransactionResponse($"{c.CustomerNumber.Value}:{c.ProductNumber.Value}", // Todo: Check for duplicate ids? I saw a warning in browser console
                                                                               c.CustomerId.Value,
                                                                               c.ProductNumber.Value,
                                                                               c.StationName.Value,
                                                                               c.Quantity.Value,
                                                                               c.CustomerNumber.Value,
                                                                               c.Country.Value,
                                                                               c.CustomerName.Value,
                                                                               c.ProductName.Value,
                                                                               c.Location.Value,
                                                                               c.LocationId.Value,
                                                                               c.CustomerType.Value,
                                                                               c.CustomerSegment.Value,
                                                                               c.AllocatedQuantity.Value,
                                                                               c.AlreadyAllocatedPercentage.Value,
                                                                               c.MissingAllocationQuantity.Value));

            return new GetOutgoingFuelTransactionsQueryResponse(result.ToList().AsReadOnly(), items.Count == request.PaginationParameters.PageSize, totalCount, totalQuantity);
        }
    }


    internal class GetOutgoingFuelTransactionsQueryAuthorizer : IAuthorizer<GetOutgoingFuelTransactionsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetOutgoingFuelTransactionsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetOutgoingFuelTransactionsQuery query,
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
