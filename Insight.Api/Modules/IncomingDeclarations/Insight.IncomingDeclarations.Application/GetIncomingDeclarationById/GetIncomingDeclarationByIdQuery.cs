using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationById
{
    public sealed class
        GetIncomingDeclarationByIdQuery : IQuery<IncomingDeclarationDto>
    {
        public IncomingDeclarationId IncomingDeclarationId { get; set; }

        private GetIncomingDeclarationByIdQuery(
            IncomingDeclarationId incomingDeclarationId)
        {
            IncomingDeclarationId = incomingDeclarationId;
        }

        public static GetIncomingDeclarationByIdQuery Create(
            IncomingDeclarationId incomingDeclarationId
        )
        {
            return new GetIncomingDeclarationByIdQuery(
                incomingDeclarationId);
        }
    }

    internal class GetIncomingDeclarationByIdQueryHandler : IQueryHandler<
        GetIncomingDeclarationByIdQuery, IncomingDeclarationDto>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;

        public GetIncomingDeclarationByIdQueryHandler(
            IIncomingDeclarationRepository incomingDeclarationRepository)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
        }

        public async Task<IncomingDeclarationDto> Handle(
            GetIncomingDeclarationByIdQuery query,
            CancellationToken cancellationToken)
        {
            var incomingDeclaration =
                await incomingDeclarationRepository.GetById(
                    query.IncomingDeclarationId.Value,
                    cancellationToken);

            if (incomingDeclaration == null)
            {
                throw new NotFoundException("Declaration not found");
            }

            var mappedDeclaration = MapToIncomingDeclarationResponse(incomingDeclaration);

            return mappedDeclaration;
        }

        private static IncomingDeclarationDto MapToIncomingDeclarationResponse(
            IncomingDeclaration incomingDeclaration)
        {
            return new IncomingDeclarationDto()
            {
                IncomingDeclarationId = incomingDeclaration.IncomingDeclarationId.Value,
                Company = incomingDeclaration.Company.Value,
                Country = incomingDeclaration.Country.Value,
                Product = incomingDeclaration.Product.Value,
                Supplier = incomingDeclaration.Supplier.Value,
                RawMaterial = incomingDeclaration.RawMaterial.Value,
                PosNumber = incomingDeclaration.PosNumber.Value,
                CountryOfOrigin = incomingDeclaration.CountryOfOrigin.Value,
                DateOfDispatch = incomingDeclaration.DateOfDispatch.Value,
                CertificationSystem = incomingDeclaration.CertificationSystem.Value,
                SupplierCertificateNumber = incomingDeclaration.SupplierCertificateNumber.Value,
                DateOfIssuance = incomingDeclaration.DateOfIssuance.Value,
                PlaceOfDispatch = incomingDeclaration.PlaceOfDispatch.Value,
                ProductionCountry = incomingDeclaration.ProductionCountry.Value,
                DateOfInstallation = incomingDeclaration.DateOfInstallation.Value,
                TypeOfProduct = incomingDeclaration.TypeOfProduct.Value,
                AdditionalInformation = incomingDeclaration.AdditionalInformation.Value,
                Quantity = incomingDeclaration.Quantity.Value,
                SpecifyNUTS2Region = incomingDeclaration.SpecifyNUTS2Region.Value,
                EnergyContentMJ = incomingDeclaration.EnergyContentMJ.Value,
                EnergyQuantityGJ = incomingDeclaration.EnergyQuantityGJ.Value,
                ComplianceWithSustainabilityCriteria =
                    incomingDeclaration.ComplianceWithSustainabilityCriteria.Value,
                CultivatedAsIntermediateCrop = incomingDeclaration.CultivatedAsIntermediateCrop.Value,
                FulfillsMeasuresForLowILUCRiskFeedstocks = incomingDeclaration
                    .FulfillsMeasuresForLowILUCRiskFeedstocks.Value,
                MeetsDefinitionOfWasteOrResidue =
                    incomingDeclaration.MeetsDefinitionOfWasteOrResidue.Value,
                GHGEec = incomingDeclaration.GHGEec.Value,
                GHGEl = incomingDeclaration.GHGEl.Value,
                GHGEp = incomingDeclaration.GHGEp.Value,
                GHGEtd = incomingDeclaration.GHGEtd.Value,
                GHGEu = incomingDeclaration.GHGEu.Value,
                GHGEsca = incomingDeclaration.GHGEsca.Value,
                GHGEccs = incomingDeclaration.GHGEccs.Value,
                GHGEccr = incomingDeclaration.GHGEccr.Value,
                GHGEee = incomingDeclaration.GHGEee.Value,
                GHGEmissionSaving = incomingDeclaration.GhgEmissionSaving.Value,
                DeclarationRowNumber = incomingDeclaration.DeclarationRowNumber.Value,
                IncomingDeclarationUploadId = incomingDeclaration.IncomingDeclarationUploadId.Value,
                IncomingDeclarationState = incomingDeclaration.IncomingDeclarationState,
                TotalDefaultValueAccordingToREDII = incomingDeclaration.TotalDefaultValueAccordingToREDII.Value,
                GHGgCO2EqPerMJ = incomingDeclaration.GHGgCO2EqPerMJ.Value,
                FossilFuelComparatorgCO2EqPerMJ = incomingDeclaration.FossilFuelComparatorgCO2EqPerMJ.Value,
                RemainingVolume = incomingDeclaration.RemainingVolume
            };
        }

        internal class GetIncomingDeclarationByIdQueryAuthorizer : IAuthorizer<GetIncomingDeclarationByIdQuery>
        {
            private readonly IExecutionContext executionContext;

            public GetIncomingDeclarationByIdQueryAuthorizer(IExecutionContext executionContext)
            {
                this.executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(GetIncomingDeclarationByIdQuery query,
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
}