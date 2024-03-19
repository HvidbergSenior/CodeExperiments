using Ganss.Excel;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;

namespace Insight.IncomingDeclarations.Infrastructure.Parser
{
    public class BFEFormat
    {
        private string[] booleans = { "y", "Y", "j", "J", "yes", "no", "YES", "NO", "Yes", "No", "JA", "NEJ", "Ja", "Nej", "True", "False", "Sandt", "Falsk", "N", "n" };

        [Column(Letter ="A")]
        public string Company { get; set; } = string.Empty;
        [Column(Letter = "B")]
        public string Country { get; set; } = string.Empty;
        [Column(Letter = "C")]
        public string Product { get; set; } = string.Empty;

        [Column(Letter = "D")]
        [DataFormat("dd-MM-yyyy")]
        public DateOnly DateOfDispatch { get; set; }
        [Column(Letter = "E")]
        public string Supplier { get; set; } = string.Empty;
        [Column(Letter = "F")]
        public string CertificationSystem { get; set; } = string.Empty;
        [Column(Letter = "G")]
        public string SupplierCertificateNumber { get; set; } = string.Empty;
        [Column(Letter = "H")]
        public string PoSNumber { get; set; } = string.Empty;
        [Column(Letter = "I")]
        public DateOnly DateOfIssuance { get; set; } = DateOnly.MinValue;
        [Column(Letter = "J")]
        public string PlaceOfDispatch { get; set; } = string.Empty;
        [Column(Letter = "K")]
        public string ProductionCountry { get; set; } = string.Empty;
        [Column(Letter = "L")]
        public string DateOfInstallation { get; set; } = string.Empty;
        [Column(Letter = "M")]
        public string TypeOfProduct { get; set; } = string.Empty;
        [Column(Letter = "N")]
        public string RawMaterial { get; set; } = string.Empty;
        [Column(Letter = "O")]
        public string AdditionalInformation { get; set; } = string.Empty;
        [Column(Letter = "P")]
        public string CountryOfOrigin { get; set; } = string.Empty;
        [Column(Letter = "Q")]
        public decimal Quantity { get; set; }
        [Column(Letter = "R")]
        public string UnitOfMeasurementString { get; set; } = string.Empty;
        public UnitOfMeasurement UnitOfMeasurement
        {
            get
            {
                switch(UnitOfMeasurementString)
                {
                    case "KG":
                        return UnitOfMeasurement.Kilograms;
                    case "M3":
                        return UnitOfMeasurement.CubicMeters;
                    case "MT":
                        return UnitOfMeasurement.MetricTon;
                    case "L":
                    default:
                        return UnitOfMeasurement.Litres;
                }
            }
        }

        [Column(Letter = "S")]
        public decimal EnergyContentMJ { get; set; }
        
        [Column(Letter = "T")]
        public string ComplianceWithEuRedMaterialString { get; set; } = string.Empty;
        public bool ComplianceWithEuRedMaterialCriteria => ComplianceWithEuRedMaterialString.In(booleans);

        [Column(Letter = "U")] 
        public string ComplianceWithIsccMaterialString { get; set; } = string.Empty;
        public bool ComplianceWithIsccMaterialCriteria => ComplianceWithIsccMaterialString.In(booleans);


        [Column(Letter = "V")] 
        public string ChainOfCustodyOption { get; set; } = string.Empty;
       
        [Column(Letter = "W")]
        public string ComplianceWithSustainabilityCriteriaString { get; set; } = string.Empty;
        
        public bool ComplianceWithSustainabilityCriteria => ComplianceWithSustainabilityCriteriaString.In(booleans);

        [Column(Letter = "X")]
        public string CultivatedAsIntermediateCropString { get; set; } = string.Empty;

        public bool CultivatedAsIntermediateCrop => CultivatedAsIntermediateCropString.In(booleans);

        [Column(Letter = "Y")]
        public string FulfillsMeasuresForLowILUCRiskFeedstocksString { get; set; } = string.Empty;
        public bool FulfillsMeasuresForLowILUCRiskFeedstocks => FulfillsMeasuresForLowILUCRiskFeedstocksString.In(booleans);

        [Column(Letter = "Z")]
        public string MeetsDefinitionOfWasteOrResidueString { get; set; } = string.Empty;
        public bool MeetsDefinitionOfWasteOrResidue => MeetsDefinitionOfWasteOrResidueString.In(booleans);


        [Column(Letter = "AA")]
        public string SpecifyNUTS2Region { get; set; } = string.Empty;


        [Column(Letter = "AB")]
        public string TotalDefaultValueAccordingToREDIIString { get; set; } = string.Empty;
        public bool TotalDefaultValueAccordingToREDII => TotalDefaultValueAccordingToREDIIString.In(booleans);


        [Column(Letter = "AC")]
        public decimal GHGEec { get; set; }
        [Column(Letter = "AD")]
        public decimal GHGEl { get; set; }
        [Column(Letter = "AE")]
        public decimal GHGEp { get; set; }
        [Column(Letter = "AF")]
        public decimal GHGEtd { get; set; }
        [Column(Letter = "AG")]
        public decimal GHGEu { get; set; }
        [Column(Letter = "AH")]
        public decimal GHGEsca { get; set; }
        [Column(Letter = "AI")]
        public decimal GHGEccs { get; set; }
        [Column(Letter = "AJ")]
        public decimal GHGEccr { get; set; }
        [Column(Letter = "AK")]
        public decimal GHGEee { get; set; }
        [Column(Letter = "AL")]
        public decimal GHGgCO2EqPerMJ { get; set; }
        [Column(Letter = "AM")]
        public decimal FossilFuelComparatorgCO2EqPerMJ { get; set; }
        [Column(Letter = "AN")]
        public decimal GHGEmissionSaving { get; set; }

        public int RowNumber { get; set; }

        public IncomingDeclaration ToIncomingDeclaration(IReadOnlyList<RawMaterialTranslation> rawMaterialTranslations, IReadOnlyList<ProductTranslation> productTranslations)
        {
            var rawMaterialValue = rawMaterialTranslations.FirstOrDefault(c => c.RawMaterialVariants.Any(desc => StringComparer.OrdinalIgnoreCase.Equals(desc.Value, RawMaterial)))?.RawMaterialStandard.Value;
            var rawMaterial = !string.IsNullOrWhiteSpace(rawMaterialValue) ? Domain.Incoming.RawMaterial.Create(rawMaterialValue) : Domain.Incoming.RawMaterial.Create(RawMaterial);

            var productValue = productTranslations.FirstOrDefault(c => c.ProductVariants.Any(desc => StringComparer.OrdinalIgnoreCase.Equals(desc.Value, Product)))?.ProductStandard.Value;
            var product = !string.IsNullOrWhiteSpace(productValue) ? Domain.Incoming.Product.Create(productValue) : Domain.Incoming.Product.Create(Product);

            return IncomingDeclaration.Create(IncomingDeclarationId.Create(Guid.NewGuid()),
                                              Domain.Incoming.Company.Create(Company),
                                              Domain.Incoming.Country.Create(Country),
                                              product,
                                              Domain.Incoming.DateOfDispatch.Create(DateOfDispatch),
                                              Domain.Incoming.Supplier.Create(Supplier),
                                              Domain.Incoming.CertificationSystem.Create(CertificationSystem),
                                              Domain.Incoming.SupplierCertificateNumber.Create(SupplierCertificateNumber),
                                              PosNumber.Create(PoSNumber),
                                              Domain.Incoming.DateOfIssuance.Create(DateOfIssuance),
                                              Domain.Incoming.PlaceOfDispatch.Create(PlaceOfDispatch),
                                              Domain.Incoming.ProductionCountry.Create(ProductionCountry),
                                              Domain.Incoming.DateOfInstallation.Create(DateOfInstallation),
                                              Domain.Incoming.TypeOfProduct.Create(TypeOfProduct),
                                              rawMaterial,
                                              Domain.Incoming.AdditionalInformation.Create(AdditionalInformation),
                                              Domain.Incoming.CountryOfOrigin.Create(CountryOfOrigin),
                                              Domain.Incoming.Quantity.Create(Quantity),
                                              UnitOfMeasurement,
                                              Domain.Incoming.EnergyContentMJ.Create(EnergyContentMJ),
                                              Domain.Incoming.EnergyQuantityGJ.Create(0), //Missing,
                                              Domain.Incoming.ComplianceWithSustainabilityCriteria.Create(ComplianceWithSustainabilityCriteria),
                                              Domain.Incoming.ComplianceWithEuRedMaterialCriteria.Create(ComplianceWithEuRedMaterialCriteria),
                                              Domain.Incoming.ComplianceWithIsccMaterialCriteria.Create(ComplianceWithIsccMaterialCriteria),
                                              Domain.Incoming.ChainOfCustodyOption.Create(ChainOfCustodyOption),
                                              Domain.Incoming.CultivatedAsIntermediateCrop.Create(CultivatedAsIntermediateCrop),
                                              Domain.Incoming.FulfillsMeasuresForLowILUCRiskFeedstocks.Create(FulfillsMeasuresForLowILUCRiskFeedstocks),
                                              Domain.Incoming.MeetsDefinitionOfWasteOrResidue.Create(MeetsDefinitionOfWasteOrResidue),
                                              NUTS2Region.Create(SpecifyNUTS2Region),
                                              TotalDefaultValueAccordingToRED2.Create(TotalDefaultValueAccordingToREDII),
                                              Domain.Incoming.GHGEec.Create(GHGEec),
                                              Domain.Incoming.GHGEl.Create(GHGEl),
                                              Domain.Incoming.GHGEp.Create(GHGEp),
                                              Domain.Incoming.GHGEtd.Create(GHGEtd),
                                              Domain.Incoming.GHGEu.Create(GHGEu),
                                              Domain.Incoming.GHGEsca.Create(GHGEsca),
                                              Domain.Incoming.GHGEccs.Create(GHGEccs),
                                              Domain.Incoming.GHGEccr.Create(GHGEccr),
                                              Domain.Incoming.GHGEee.Create(GHGEee),
                                              GHGgCO2eqPerMJ.Create(GHGgCO2EqPerMJ),
                                              FossilFuelComparatorgCO2eqPerMJ.Create(FossilFuelComparatorgCO2EqPerMJ),
                                              Domain.Incoming.GHGEmissionSaving.Create(GHGEmissionSaving),
                                              DeclarationRowNumber.Create(RowNumber),
                                              IncomingDeclarationUploadId.Empty(),
                                              IncomingDeclarationState.Temporary,
                                              SourceFormatPropertyBag.Empty());
        }
    }
}
