using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;

namespace Insight.FuelTransactions.Application.GetGroupedFuelTransactions
{
    internal class GetGroupedFuelTransactionsQueryHandler : IQueryHandler<GetGroupedFuelTransactionsQuery, GetGroupedFuelTransactionsDto>
    {
        private readonly IFuelTransactionsRepository fuelTransactionsRepository;

        public GetGroupedFuelTransactionsQueryHandler(IFuelTransactionsRepository fuelTransactionsRepository)
        {
            this.fuelTransactionsRepository = fuelTransactionsRepository;
        }

        public async Task<GetGroupedFuelTransactionsDto> Handle(GetGroupedFuelTransactionsQuery request, CancellationToken cancellationToken)
        {
            var groupedTransactions = await fuelTransactionsRepository.GetGroupedTransactions(
                request.SustainabilityAndFuelConsumptionFilteringParameters.DatePeriod,
                request.SustainabilityAndFuelConsumptionFilteringParameters.ProductNames,
                request.SustainabilityAndFuelConsumptionFilteringParameters.CustomerIds);
            
            var getGroupedFuelTransactions = new List<GetGroupedFuelTransactionDto>();
            
            foreach (var transaction in groupedTransactions)
            {
                getGroupedFuelTransactions.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
            }
            
            return new GetGroupedFuelTransactionsDto(getGroupedFuelTransactions);
        }
    }

    internal class GetGroupedFuelTransactionsQueryAuthorizer : IAuthorizer<GetGroupedFuelTransactionsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetGroupedFuelTransactionsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetGroupedFuelTransactionsQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }

            if (!query.SustainabilityAndFuelConsumptionFilteringParameters.CustomerIds.Any())
            {
                return AuthorizationResult.Fail();    
            }
            
            var affectedCustomerIds = query.SustainabilityAndFuelConsumptionFilteringParameters.CustomerIds;
            var myCustomerIds = (await executionContext.GetCustomersPermissionsAsync(true, cancellation)).Select(c=> c.CustomerId)!;
            if (affectedCustomerIds.Except(myCustomerIds).Any())
            {
                return AuthorizationResult.Fail();    
            }
            return AuthorizationResult.Succeed();
        }
    }
}
