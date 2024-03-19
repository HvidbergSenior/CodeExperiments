using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration
{
    public class UpdateIncomingDeclarationRequest
    {
        [Required(ErrorMessage = "UpdateIncomingDeclarationId is required")]
        public required Guid UpdateIncomingDeclarationId { get; set; }
     
        [Required(ErrorMessage = "Company is required")]
        public required string Company { get; set; }

        [Required(ErrorMessage = "Country  is required")]
        public required string Country { get; set; }
        
        [Required(ErrorMessage = "Product  is required")]
        public required string Product { get; set; }
        
        [Required(ErrorMessage = "Supplier  is required")]
        public required string Supplier { get; set; }
        
        [Required(ErrorMessage = "Raw Material is required")]
        public required string RawMaterial { get; set; }
        
        [Required(ErrorMessage = "PosNumber  is required")]
        public required string PosNumber { get; set; }
        
        [Required(ErrorMessage = "CountryOfOrigin  is required")]
        public required string CountryOfOrigin { get; set; }
        
        [Required(ErrorMessage = "IncomingDeclarationState  is required")]
        public required IncomingDeclarationState IncomingDeclarationState { get; set; }
        [Required] 
        public required DateOnly DateOfDispatch { get; set; }
        [Required] 
        public required string CertificationSystem { get; set; }
        [Required] 
        public required string SupplierCertificateNumber { get; set; }
        [Required] 
        public required DateOnly DateOfIssuance { get; set; }
        [Required] 
        public required string PlaceOfDispatch { get; set; }
        [Required] 
        public required string ProductionCountry { get; set; }
        [Required] 
        public required DateOnly DateOfInstallation { get; set; }
        [Required] 
        public required string TypeOfProduct { get; set; }
        [Required] 
        public required string AdditionalInformation { get; set; }
        [Required] 
        public required decimal Quantity { get; set; }
        [Required] 
        public UnitOfMeasurement UnitOfMeasurement { get;  set; }
        [Required] 
        public required decimal EnergyContentMJ { get; set; }
        [Required] 
        public required decimal EnergyQuantityGJ { get; set; }
        [Required] 
        public required bool ComplianceWithSustainabilityCriteria { get; set; }
        [Required] 
        public required bool CultivatedAsIntermediateCrop { get; set; }
        [Required] 
        public required bool FulfillsMeasuresForLowILUCRiskFeedstocks { get; set; }
        [Required] 
        public required bool MeetsDefinitionOfWasteOrResidue { get; set; }
        [Required] 
        public required string NUTS2Region { get; set; }
        
        [Required] 
        public required bool TotalDefaultValueAccordingToREDII { get; set; }
        [Required] 
        public required decimal GHGEec { get; set; }
        [Required] 
        public required decimal GHGEl { get; set; }
        [Required] 
        public required decimal GHGEp { get; set; }
        [Required] 
        public required decimal GHGEtd { get; set; }
        [Required] 
        public required decimal GHGEu { get; set; }
        [Required] 
        public required decimal GHGEsca { get; set; }
        [Required] 
        public required decimal GHGEccs { get; set; }
        [Required] 
        public required decimal GHGEccr { get; set; }
        [Required] 
        public required decimal GHGEee { get; set; }
        [Required] 
        public required decimal GHGgCO2EqPerMJ { get; set; }
        [Required] 
        public required decimal FossilFuelComparatorgCO2EqPerMJ { get; set; }
        [Required] 
        public required decimal GHGEmissionSaving { get; set; }
        [Required] 
        public required int DeclarationRowNumber { get; set; }
        [Required] 
        public required Guid IncomingDeclarationUploadId { get; set; }
    }
}