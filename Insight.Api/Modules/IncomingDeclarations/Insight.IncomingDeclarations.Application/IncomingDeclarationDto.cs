using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application
{
    public class IncomingDeclarationDto
    {
        [Required] public Guid IncomingDeclarationId { get; set; }
        [Required] public Guid IncomingDeclarationUploadId { get; set; }
        [Required] public string Company { get; set; } = string.Empty;
        [Required] public string Country { get; set; } = string.Empty;
        [Required] public string Product { get; set; } = string.Empty;
        [Required] public string Supplier { get; set; } = string.Empty;
        [Required] public string RawMaterial { get; set; } = string.Empty;
        [Required] public string PosNumber { get; set; } = string.Empty;
        [Required] public string CountryOfOrigin { get; set; } = string.Empty;
        [Required] public IncomingDeclarationState IncomingDeclarationState { get; set; }
        [Required] public DateOnly DateOfDispatch { get; set; }
        [Required] public string CertificationSystem { get; set; } = string.Empty;
        [Required] public string SupplierCertificateNumber { get; set; } = string.Empty;
        [Required] public DateOnly DateOfIssuance { get; set; }
        [Required] public string PlaceOfDispatch { get; set; } = string.Empty;
        [Required] public string ProductionCountry { get; set; } = string.Empty;
        [Required] public string DateOfInstallation { get; set; } = string.Empty;
        [Required] public string TypeOfProduct { get; set; } = string.Empty;
        [Required] public string AdditionalInformation { get; set; } = string.Empty;
        [Required] public decimal Quantity { get; set; }
        [Required] public UnitOfMeasurement UnitOfMeasurement { get; set; }
        [Required] public decimal EnergyContentMJ { get; set; }
        [Required] public decimal EnergyQuantityGJ { get; set; }
        [Required] public bool ComplianceWithSustainabilityCriteria { get; set; }
        [Required] public bool CultivatedAsIntermediateCrop { get; set; }
        [Required] public bool FulfillsMeasuresForLowILUCRiskFeedstocks { get; set; }
        [Required] public bool MeetsDefinitionOfWasteOrResidue { get; set; }
        [Required] public string SpecifyNUTS2Region { get; set; } = string.Empty;
        [Required] public bool TotalDefaultValueAccordingToREDII { get; set; }
        [Required] public decimal GHGEec { get; set; }
        [Required] public decimal GHGEl { get; set; }
        [Required] public decimal GHGEp { get; set; }
        [Required] public decimal GHGEtd { get; set; }
        [Required] public decimal GHGEu { get; set; }
        [Required] public decimal GHGEsca { get; set; }
        [Required] public decimal GHGEccs { get; set; }
        [Required] public decimal GHGEccr { get; set; }
        [Required] public decimal GHGEee { get; set; }
        [Required] public decimal GHGgCO2EqPerMJ { get; set; }
        [Required] public decimal FossilFuelComparatorgCO2EqPerMJ { get; set; }
        [Required] public decimal GHGEmissionSaving { get; set; }
        [Required] public int DeclarationRowNumber { get; set; }
        [Required] public decimal RemainingVolume { get; set; }
    }
}