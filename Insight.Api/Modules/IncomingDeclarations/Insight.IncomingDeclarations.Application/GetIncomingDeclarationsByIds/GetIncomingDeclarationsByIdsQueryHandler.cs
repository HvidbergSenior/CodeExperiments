using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByIds
{
    internal class GetIncomingDeclarationsByIdsQueryHandler : IQueryHandler<GetIncomingDeclarationsByIdsQuery, GetIncomingDeclarationsDto>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;

        public GetIncomingDeclarationsByIdsQueryHandler(IIncomingDeclarationRepository incomingDeclarationRepository)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
        }

        public async Task<GetIncomingDeclarationsDto> Handle(GetIncomingDeclarationsByIdsQuery request, CancellationToken cancellationToken)
        {
            var incomingDeclarations = await incomingDeclarationRepository.GetByIdsAsync(request.IncomingDeclarationIds, cancellationToken);

            var getIncomingDeclarations = new List<GetIncomingDeclarationDto>();
            
            foreach (var incomingDeclaration in incomingDeclarations)
            {
                getIncomingDeclarations.Add(new GetIncomingDeclarationDto(
                    incomingDeclaration.IncomingDeclarationId.Value,
                    incomingDeclaration.Company.Value,
                    incomingDeclaration.Country.Value,
                    incomingDeclaration.Product.Value, incomingDeclaration.Supplier.Value,
                    incomingDeclaration.RawMaterial.Value, incomingDeclaration.PosNumber.Value,
                    incomingDeclaration.CountryOfOrigin.Value, incomingDeclaration.PlaceOfDispatch.Value,
                    incomingDeclaration.DateOfDispatch.Value, 
                    incomingDeclaration.Quantity.Value,
                    incomingDeclaration.GhgEmissionSaving.Value, 
                    //BatchId is only used when creating an OutgoingDeclaration
                    0, 
                    incomingDeclaration.DateOfIssuance.Value,
                    incomingDeclaration.DateOfInstallation.Value,
                    incomingDeclaration.ProductionCountry.Value,
                    incomingDeclaration.SupplierCertificateNumber.Value,
                    incomingDeclaration.CertificationSystem.Value,
                    incomingDeclaration.ComplianceWithSustainabilityCriteria.Value,
                    incomingDeclaration.ComplianceWithEuRedMaterialCriteria.Value,
                    incomingDeclaration.ComplianceWithIsccMaterialCriteria.Value,
                    incomingDeclaration.ChainOfCustodyOption.Value,
                    incomingDeclaration.TotalDefaultValueAccordingToREDII.Value,
                    incomingDeclaration.TypeOfProduct.Value,
                    incomingDeclaration.EnergyContentMJ.Value,
                    incomingDeclaration.CultivatedAsIntermediateCrop.Value,
                    incomingDeclaration.FulfillsMeasuresForLowILUCRiskFeedstocks.Value,
                    incomingDeclaration.MeetsDefinitionOfWasteOrResidue.Value,
                    incomingDeclaration.SpecifyNUTS2Region.Value,
                    incomingDeclaration.GHGEccs.Value,
                    incomingDeclaration.GHGEccr.Value,
                    incomingDeclaration.GHGEec.Value,
                    incomingDeclaration.GHGEu.Value,
                    incomingDeclaration.GHGEl.Value,
                    incomingDeclaration.GHGEp.Value,
                    incomingDeclaration.GHGEsca.Value,
                    incomingDeclaration.GHGgCO2EqPerMJ.Value,
                    incomingDeclaration.GHGEtd.Value,
                    incomingDeclaration.GHGgCO2EqPerMJ.Value,
                    incomingDeclaration.FossilFuelComparatorgCO2EqPerMJ.Value
                ));
            }
            return new GetIncomingDeclarationsDto(getIncomingDeclarations);
        }
    }

    internal class GetIncomingDeclarationsByIdsQueryAuthorizer : IAuthorizer<GetIncomingDeclarationsByIdsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetIncomingDeclarationsByIdsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(GetIncomingDeclarationsByIdsQuery query,
            CancellationToken cancellation)
        {
           //TODO: This handler is called from 2 places ->
           //1) GetOutgoingDeclarationByID -> the attached IncomingDeclarations the user can see as well
           //2) GetSustainabilityReportPdf -> again to find the IncomingDeclarations attached to the Outgoingdeclaration
           //If we need to check auth here, perhaps we should make one handler for each
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
