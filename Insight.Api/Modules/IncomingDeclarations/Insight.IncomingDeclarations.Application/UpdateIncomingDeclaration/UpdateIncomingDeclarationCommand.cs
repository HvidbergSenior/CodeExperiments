using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using System.Threading;

namespace Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration
{
    public sealed class UpdateIncomingDeclarationCommand : ICommand<UpdateIncomingDeclarationResponse>
    {
        internal IncomingDeclarationId IncomingDeclarationId { get; }
        internal IncomingDeclarationUpdateParameters IncomingDeclarationUpdateParameters { get; }

        private UpdateIncomingDeclarationCommand(IncomingDeclarationId incomingDeclarationId,
            IncomingDeclarationUpdateParameters incomingDeclarationUpdateParameters)
        {
            IncomingDeclarationId = incomingDeclarationId;
            IncomingDeclarationUpdateParameters = incomingDeclarationUpdateParameters;
        }

        public static UpdateIncomingDeclarationCommand Create(Guid incomingDeclarationId,
            IncomingDeclarationUpdateParameters incomingDeclarationUpdateParameters)
        {
            return new UpdateIncomingDeclarationCommand(IncomingDeclarationId.Create(incomingDeclarationId),
                IncomingDeclarationUpdateParameters.Create(
                    Company.Create(incomingDeclarationUpdateParameters.Company.Value),
                    Country.Create(incomingDeclarationUpdateParameters.Country.Value),
                    Product.Create(incomingDeclarationUpdateParameters.Product.Value),
                    Supplier.Create(incomingDeclarationUpdateParameters.Supplier.Value),
                    RawMaterial.Create(incomingDeclarationUpdateParameters.RawMaterial.Value),
                    PosNumber.Create(incomingDeclarationUpdateParameters.PosNumber.Value),
                    CountryOfOrigin.Create(incomingDeclarationUpdateParameters.CountryOfOrigin.Value),
                    DateOfDispatch.Create(incomingDeclarationUpdateParameters.DateOfDispatch.Value),
                    CertificationSystem.Create(incomingDeclarationUpdateParameters.CertificationSystem.Value),
                    SupplierCertificateNumber.Create(incomingDeclarationUpdateParameters.SupplierCertificateNumber.Value),
                    DateOfIssuance.Create(incomingDeclarationUpdateParameters.DateOfIssuance.Value),
                    PlaceOfDispatch.Create(incomingDeclarationUpdateParameters.PlaceOfDispatch.Value),
                    ProductionCountry.Create(incomingDeclarationUpdateParameters.ProductionCountry.Value),
                    DateOfInstallation.Create(incomingDeclarationUpdateParameters.DateOfInstallation.Value),
                    TypeOfProduct.Create(incomingDeclarationUpdateParameters.TypeOfProduct.Value),
                    AdditionalInformation.Create(incomingDeclarationUpdateParameters.AdditionalInformation.Value),
                    Quantity.Create(incomingDeclarationUpdateParameters.Quantity.Value),
                    incomingDeclarationUpdateParameters.UnitOfMeasurement,
                    EnergyContentMJ.Create(incomingDeclarationUpdateParameters.EnergyContentMJ.Value),
                    EnergyQuantityGJ.Create(incomingDeclarationUpdateParameters.EnergyQuantityGJ.Value),
                    ComplianceWithSustainabilityCriteria.Create(incomingDeclarationUpdateParameters.ComplianceWithSustainabilityCriteria.Value),
                    CultivatedAsIntermediateCrop.Create(incomingDeclarationUpdateParameters.CultivatedAsIntermediateCrop.Value),
                    FulfillsMeasuresForLowILUCRiskFeedstocks.Create(incomingDeclarationUpdateParameters.FulfillsMeasuresForLowILUCRiskFeedstocks.Value),
                    MeetsDefinitionOfWasteOrResidue.Create(incomingDeclarationUpdateParameters.MeetsDefinitionOfWasteOrResidue.Value),
                    NUTS2Region.Create(incomingDeclarationUpdateParameters.NUTS2Region.Value), 
                    TotalDefaultValueAccordingToRED2.Create(incomingDeclarationUpdateParameters.TotalDefaultValueAccordingToRED2.Value),
                    GHGEec.Create(incomingDeclarationUpdateParameters.GHGEec.Value),
                    GHGEl.Create(incomingDeclarationUpdateParameters.GHGEl.Value),
                    GHGEp.Create(incomingDeclarationUpdateParameters.GHGEp.Value),
                    GHGEtd.Create(incomingDeclarationUpdateParameters.GHGEtd.Value),
                    GHGEu.Create(incomingDeclarationUpdateParameters.GHGEu.Value),
                    GHGEsca.Create(incomingDeclarationUpdateParameters.GHGEsca.Value),
                    GHGEccs.Create(incomingDeclarationUpdateParameters.GHGEccs.Value),
                    GHGEccr.Create(incomingDeclarationUpdateParameters.GHGEccr.Value),
                    GHGEee.Create(incomingDeclarationUpdateParameters.GHGEee.Value),
                    GHGgCO2eqPerMJ.Create(incomingDeclarationUpdateParameters.GHGgCO2eqPerMJ.Value),
                    FossilFuelComparatorgCO2eqPerMJ.Create(incomingDeclarationUpdateParameters.FossilFuelComparatorgCO2eqPerMJ.Value),
                    GHGEmissionSaving.Create(incomingDeclarationUpdateParameters.GHGEmissionSaving.Value),
                    DeclarationRowNumber.Create(incomingDeclarationUpdateParameters.DeclarationRowNumber.Value),
                    IncomingDeclarationUploadId.Create(incomingDeclarationUpdateParameters.IncomingDeclarationUploadId.Value)
                ));
        }
    }

    internal class UpdateIncomingDeclarationCommandHandler : ICommandHandler<UpdateIncomingDeclarationCommand,
            UpdateIncomingDeclarationResponse>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly IUnitOfWork unitOfWork;
        public UpdateIncomingDeclarationCommandHandler(IIncomingDeclarationRepository incomingDeclarationRepository,
            IUnitOfWork unitOfWork)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<UpdateIncomingDeclarationResponse> Handle(UpdateIncomingDeclarationCommand request,
            CancellationToken cancellationToken)
        {
            var declaration = await incomingDeclarationRepository.GetById(request.IncomingDeclarationId.Value);

            if (declaration == null)
            {
                throw new NotFoundException("Declaration not found");
            }

            declaration.SetUpdatedIncomingDeclaration(request.IncomingDeclarationUpdateParameters);
            
            await incomingDeclarationRepository.Update(declaration, cancellationToken);
            await incomingDeclarationRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return MapToUpdateIncomingDeclarationResponses(declaration);
        }

        private static UpdateIncomingDeclarationResponse MapToUpdateIncomingDeclarationResponses(
            IncomingDeclaration declaration)
        {
            return new UpdateIncomingDeclarationResponse(
                declaration.IncomingDeclarationId.Value,
                declaration.Company.Value,
                declaration.Country.Value,
                declaration.Product.Value,
                declaration.Supplier.Value,
                declaration.RawMaterial.Value,
                declaration.CountryOfOrigin.Value,
                declaration.PosNumber.Value,
                declaration.IncomingDeclarationState);
        }
    }

    internal class UpdateIncomingDeclarationCommandAuthorizer : IAuthorizer<UpdateIncomingDeclarationCommand>
    {
        private readonly IExecutionContext executionContext;

        public UpdateIncomingDeclarationCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateIncomingDeclarationCommand command,
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