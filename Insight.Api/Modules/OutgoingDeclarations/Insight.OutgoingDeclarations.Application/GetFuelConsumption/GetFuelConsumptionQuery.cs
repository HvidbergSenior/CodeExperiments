using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.OutgoingDeclarations.Application.Helpers;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumption
{
    public sealed class GetFuelConsumptionQuery : IQuery<GetFuelConsumptionResponse>
    {
        public SustainabilityAndFuelConsumptionFilteringParameters SustainabilityAndFuelConsumptionFilteringParameters { get; private set; }

        private GetFuelConsumptionQuery(SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            SustainabilityAndFuelConsumptionFilteringParameters = filteringParameters;
        }

        public static GetFuelConsumptionQuery Create(
            SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            return new GetFuelConsumptionQuery(filteringParameters);
        }
    }

    public sealed class GetFuelConsumptionHandler : IQueryHandler<GetFuelConsumptionQuery, GetFuelConsumptionResponse>
    {
        private readonly IQueryBus queryBus;

        public GetFuelConsumptionHandler(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }
        
        public async Task<GetFuelConsumptionResponse> Handle(GetFuelConsumptionQuery request,
            CancellationToken cancellationToken)
        {
            var groupedFuelTransactions = GetGroupedFuelTransactionsQuery.Create(request.SustainabilityAndFuelConsumptionFilteringParameters);
     
            var groupedFuelTransactionsDto = await queryBus.Send<GetGroupedFuelTransactionsQuery, GetGroupedFuelTransactionsDto>(groupedFuelTransactions, cancellationToken);

            var response = new GetFuelConsumptionResponse
            {
                ConsumptionPerProduct = FuelConsumptionHelper.GetConsumptionPerProduct(groupedFuelTransactionsDto.GroupedFuelTransactionDto),
                ConsumptionDevelopment = FuelConsumptionHelper.GetConsumptionDevelopment(groupedFuelTransactionsDto.GroupedFuelTransactionDto, request.SustainabilityAndFuelConsumptionFilteringParameters.MaxColumns),
                ConsumptionStats = FuelConsumptionHelper.GetConsumptionStats(groupedFuelTransactionsDto)
            };

            return response;
        }
    }

    internal class GetFuelConsumptionQueryAuthorizer : IAuthorizer<GetFuelConsumptionQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetFuelConsumptionQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(GetFuelConsumptionQuery reportQuery,
            CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}