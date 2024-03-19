using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;
using Insight.OutgoingDeclarations.Application.Helpers;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetSustainabilityReport
{
    public sealed class GetSustainabilityReportQuery : IQuery<GetSustainabilityReportResponse>
    {
        public SustainabilityAndFuelConsumptionFilteringParameters SustainabilityAndFuelConsumptionFilteringParameters { get; private set; }


        private GetSustainabilityReportQuery(SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            SustainabilityAndFuelConsumptionFilteringParameters = filteringParameters;
        }

        public static GetSustainabilityReportQuery Create(SustainabilityAndFuelConsumptionFilteringParameters filteringParameters)
        {
            return new GetSustainabilityReportQuery(filteringParameters);
        }
    }

    public sealed class GetSustainabilityReportHandler : IQueryHandler<GetSustainabilityReportQuery, GetSustainabilityReportResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationsRepository;
        private readonly IQueryBus queryBus;


        public GetSustainabilityReportHandler(IOutgoingDeclarationRepository outgoingDeclarationsRepository, IQueryBus queryBus)
        {
            this.outgoingDeclarationsRepository = outgoingDeclarationsRepository;
            this.queryBus = queryBus;
        }

        public async Task<GetSustainabilityReportResponse> Handle(GetSustainabilityReportQuery request,
            CancellationToken cancellationToken)
        {
            //OutgoingDeclarations from this module
            var outgoingDeclarations = await outgoingDeclarationsRepository.GetOutgoingDeclarationsForMany(
                FilteringParametersSelectMany.Create(
                    request.SustainabilityAndFuelConsumptionFilteringParameters.DatePeriod,
                    request.SustainabilityAndFuelConsumptionFilteringParameters.ProductNames,
                    request.SustainabilityAndFuelConsumptionFilteringParameters.CustomerNumbers));

            //Affected Incoming Declarations from IncomingDeclarations module
            var affectedIncomingDeclarationsIds = outgoingDeclarations.SelectMany(decl =>
                decl.IncomingDeclarationPairings.Select(pairing => pairing.IncomingDeclarationId.Value)).ToList();

            var incomingDeclarationsByIdsQuery =
                GetIncomingDeclarationsByIdsQuery.Create(affectedIncomingDeclarationsIds);

            var affectedIncomingDeclarations =
                await queryBus.Send<GetIncomingDeclarationsByIdsQuery, GetIncomingDeclarationsDto>(
                    incomingDeclarationsByIdsQuery, cancellationToken);

            var feedStocks =
                SustainabilityReportPdfHelper.GetFeedstocks(outgoingDeclarations, affectedIncomingDeclarations);
            var countries =
                SustainabilityReportPdfHelper.GetCountries(outgoingDeclarations, affectedIncomingDeclarations);
            var progress =
                SustainabilityReportPdfHelper.GetProgress(outgoingDeclarations, affectedIncomingDeclarations);
            var productSpecificationItems =
                SustainabilityReportPdfHelper.GetProductSpecificationItems(outgoingDeclarations,
                    affectedIncomingDeclarations);
            var emissionStats =
                SustainabilityReportPdfHelper.GetEmissionStats(outgoingDeclarations, affectedIncomingDeclarations);

            var response = new GetSustainabilityReportResponse
            {
                Feedstocks = feedStocks,
                Countries = countries,
                Progress = progress,
                ProductSpecificationItems = productSpecificationItems,
                EmissionsStats = emissionStats
            };

            return response;
        }
    }

    internal class GetSustainabilityReportQueryAuthorizer : IAuthorizer<GetSustainabilityReportQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetSustainabilityReportQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(GetSustainabilityReportQuery reportReportQuery,
            CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}