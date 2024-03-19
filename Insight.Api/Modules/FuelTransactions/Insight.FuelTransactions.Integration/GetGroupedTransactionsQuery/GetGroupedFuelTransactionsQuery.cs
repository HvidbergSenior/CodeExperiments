using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery
{
    public sealed class GetGroupedFuelTransactionsQuery : IQuery<GetGroupedFuelTransactionsDto>
    {
        public SustainabilityAndFuelConsumptionFilteringParameters SustainabilityAndFuelConsumptionFilteringParameters { get; private set; }

        private GetGroupedFuelTransactionsQuery(SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            SustainabilityAndFuelConsumptionFilteringParameters = filteringParameters;
        }

        public static GetGroupedFuelTransactionsQuery Create(
            SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            return new GetGroupedFuelTransactionsQuery(filteringParameters);
        }
    }
}
