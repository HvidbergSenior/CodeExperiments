using Insight.BuildingBlocks.Application.Commands;
using Insight.IncomingDeclarations.Domain.Incoming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration;

public static class UpdateIncomingDeclarationEndpoint
{
    public static void MapUpdateIncomingDeclarationEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint
            .MapPut(IncomingDeclarationsEndpointUrls.UPDATE_INCOMING_DECLARATION_ENDPOINT, async (
                IncomingDeclarationDto request,
                ICommandBus commandBus,
                CancellationToken cancellationToken) =>
            {
                var incomingDeclarationUpdateParameters = CreateIncomingDeclarationUpdateParameters(request);
  
                var command = UpdateIncomingDeclarationCommand.Create(
                    request.IncomingDeclarationId,
                    incomingDeclarationUpdateParameters);
                
                var result = await commandBus.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .RequireAuthorization()
            .Produces<UpdateIncomingDeclarationResponse>()
            .WithName("UpdateIncomingDeclaration")
            .WithTags("IncomingDeclarations");
    }

    private static IncomingDeclarationUpdateParameters CreateIncomingDeclarationUpdateParameters(IncomingDeclarationDto request)
    {
        var incomingDeclarationUpdateParameters = IncomingDeclarationUpdateParameters.Create(
            Company.Create(request.Company),
            Country.Create(request.Country),
            Product.Create(request.Product),
            Supplier.Create(request.Supplier),
            RawMaterial.Create(request.RawMaterial),
            PosNumber.Create(request.PosNumber),
            CountryOfOrigin.Create(request.CountryOfOrigin),
            DateOfDispatch.Create(request.DateOfDispatch),
            CertificationSystem.Create(request.CertificationSystem),
            SupplierCertificateNumber.Create(request.SupplierCertificateNumber), 
            DateOfIssuance.Create(request.DateOfIssuance), 
            PlaceOfDispatch.Create(request.PlaceOfDispatch), 
            ProductionCountry.Create(request.ProductionCountry), 
            DateOfInstallation.Create(request.DateOfInstallation), 
            TypeOfProduct.Create(request.TypeOfProduct), 
            AdditionalInformation.Create(request.AdditionalInformation), 
            Quantity.Create(request.Quantity), 
            request.UnitOfMeasurement,
            EnergyContentMJ.Create(request.EnergyContentMJ), 
            EnergyQuantityGJ.Create(request.EnergyQuantityGJ), 
            ComplianceWithSustainabilityCriteria.Create(request.ComplianceWithSustainabilityCriteria), 
            CultivatedAsIntermediateCrop.Create(request.CultivatedAsIntermediateCrop), 
            FulfillsMeasuresForLowILUCRiskFeedstocks.Create(request.FulfillsMeasuresForLowILUCRiskFeedstocks), 
            MeetsDefinitionOfWasteOrResidue.Create(request.MeetsDefinitionOfWasteOrResidue), 
            NUTS2Region.Create(request.SpecifyNUTS2Region), 
            TotalDefaultValueAccordingToRED2.Create(request.TotalDefaultValueAccordingToREDII),
            GHGEec.Create(request.GHGEec), 
            GHGEl.Create(request.GHGEl), 
            GHGEp.Create(request.GHGEp), 
            GHGEtd.Create(request.GHGEtd), 
            GHGEu.Create(request.GHGEu), 
            GHGEsca.Create(request.GHGEsca), 
            GHGEccs.Create(request.GHGEccs), 
            GHGEccr.Create(request.GHGEccr), 
            GHGEee.Create(request.GHGEee), 
            GHGgCO2eqPerMJ.Create(request.GHGgCO2EqPerMJ),
            FossilFuelComparatorgCO2eqPerMJ.Create(request.FossilFuelComparatorgCO2EqPerMJ), 
            GHGEmissionSaving.Create(request.GHGEmissionSaving), 
            DeclarationRowNumber.Create(request.DeclarationRowNumber), 
            IncomingDeclarationUploadId.Create(request.IncomingDeclarationUploadId)
        );
        return incomingDeclarationUpdateParameters;
    }
}