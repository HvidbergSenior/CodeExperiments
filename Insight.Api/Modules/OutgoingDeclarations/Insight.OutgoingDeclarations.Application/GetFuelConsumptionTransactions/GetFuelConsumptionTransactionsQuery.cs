using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.FuelTransactions.Domain;
using Insight.OutgoingDeclarations.Domain.FuelConsumptionTransactions;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactions
{
    public sealed class GetFuelConsumptionTransactionsQuery : IQuery<GetFuelConsumptionTransactionsResponse>
    {
        public FuelConsumptionTransactionsFilteringParameters FuelConsumptionFilteringParameters { get; private set; }

        private GetFuelConsumptionTransactionsQuery(FuelConsumptionTransactionsFilteringParameters filteringParameters)
        {
            FuelConsumptionFilteringParameters = filteringParameters;
        }

        public static GetFuelConsumptionTransactionsQuery Create(
            FuelConsumptionTransactionsFilteringParameters filteringParameters)
        {
            return new GetFuelConsumptionTransactionsQuery(filteringParameters);
        }
    }

    public sealed class GetFuelConsumptionTransactionsHandler : IQueryHandler<GetFuelConsumptionTransactionsQuery, GetFuelConsumptionTransactionsResponse>
    {
        private readonly IFuelTransactionsRepository fuelTransactionsRepository;

        public GetFuelConsumptionTransactionsHandler(IFuelTransactionsRepository fuelTransactionsRepository)
        {
            this.fuelTransactionsRepository = fuelTransactionsRepository;
        }

        //public async Task<GetFuelConsumptionTransactionsResponse> Handle(GetFuelConsumptionTransactionsQuery request,
        public async Task<GetFuelConsumptionTransactionsResponse> Handle(GetFuelConsumptionTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            /*var mockResponse = new GetFuelConsumptionTransactionsResponse
            {
                Data = GetFuelConsumptionTransactionsMock(),
                HasMoreTransactions = false,
                TotalAmountOfTransactions = 2
            };

            return Task.FromResult(mockResponse);*/

            return await GetFuelConsumptionTransactions(request, cancellationToken);
        }

        private static string MaskLastCharacter(string value)
        {
            return value.Length > 0 ? $"{value.Substring(0, value.Length - 1)}*" : "";
        }

        private async Task<GetFuelConsumptionTransactionsResponse> GetFuelConsumptionTransactions(GetFuelConsumptionTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var (transactions, totalCount) = await fuelTransactionsRepository.GetFuelTransactionsRefinedAsync(request.FuelConsumptionFilteringParameters.DatePeriod,
                request.FuelConsumptionFilteringParameters.ProductNames,
                request.FuelConsumptionFilteringParameters.CustomerIds,
                request.FuelConsumptionFilteringParameters.PaginationParameters,
                request.FuelConsumptionFilteringParameters.SortingParameters,
                cancellationToken);

            var result = transactions.OrderByDescending(p => p.FuelTransactionTimeStamp).Select(c =>
                new FuelConsumptionTransaction()
                {
                    Date = c.FuelTransactionDate.Value,
                    Id = c.Id.ToString(),
                    Quantity = c.Quantity.Value,
                    Time = c.FuelTransactionTime.Value,
                    CardNumber = MaskLastCharacter(c.DriverCardNumber.Value),
                    CustomerName = c.CustomerName.Value,
                    CustomerNumber = c.CustomerNumber.Value,
                    ProductName = c.ProductDescription.Value,
                    ProductNumber = c.ProductNumber.Value,
                    StationId = c.StationNumber.Value,
                    StationName = c.StationName.Value,
                    Location = c.ShipToLocation.Value
                });

            
            var response = new GetFuelConsumptionTransactionsResponse()
            {
                Data = result.ToList().AsReadOnly(),
                TotalAmountOfTransactions = totalCount,
                HasMoreTransactions = totalCount > (request.FuelConsumptionFilteringParameters.PaginationParameters.PageSize*(request.FuelConsumptionFilteringParameters.PaginationParameters.Page))
            };

            return response;
        }

        private static List<FuelConsumptionTransaction> GetFuelConsumptionTransactionsMock()
        {
            return new List<FuelConsumptionTransaction>()
            {
                new FuelConsumptionTransaction()
                {
                    Id = "0",
                    Date = "2024-1-16",
                    Time = "217788.3904",
                    StationId = "2105",
                    StationName = "Vimmerby",
                    ProductNumber = "445",
                    ProductName = "HVO100",
                    CustomerNumber = "1234567890987",
                    CustomerName = "Arriva",
                    CardNumber = "56789",
                    Quantity = 260,
                    Location = "Vimmerby"
                },
                new FuelConsumptionTransaction()
                {
                    Id = "1",
                    Date = "2024-1-17",
                    Time = "1000000",
                    StationId = "2105",
                    StationName = "Vimmerby",
                    ProductNumber = "445",
                    ProductName = "HVO100",
                    CustomerNumber = "7890987654321",
                    CustomerName = "Arriva",
                    CardNumber = "56789",
                    Quantity = 260,
                    Location = "Vimmerby"
                }
            };
        }
    }

    //Black magic? Leave for now
    internal class GetFuelConsumptionTransactionsQueryAuthorizer : IAuthorizer<GetFuelConsumptionTransactionsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetFuelConsumptionTransactionsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(GetFuelConsumptionTransactionsQuery reportQuery,
            CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}