using System.Globalization;
using Ganss.Excel;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Infrastructure.Parser
{
    public class NesteFormat
    {
        private const string NESTE_DATE_FORMAT = "yyyyMMdd000000";
        private const UnitOfMeasurement NESTE_QUANTITY_UOM = UnitOfMeasurement.Litres;

        [Column(Letter = "C")]
        public string Supplier { get; set; } = string.Empty;
        [Column(Letter = "F")]
        public string PlaceOfDispatch { get; set; } = string.Empty;
        [Column(Letter = "I")]
        public string CountryString { get; set; } = string.Empty;

        public string Country => ParseNesteCountry(CountryString);

        private static string ParseNesteCountry(string countryString)
        {
            switch (countryString)
            {
                case "M0":
                    return "SE";
                case "W0":
                    return "DK";
                case "V0":
                    return "NO";
                default:
                    return "UNKNOWN";
            }
        }

        [Column(Letter = "L")]
        public string Company { get; set; } = string.Empty;
        [Column(Letter = "M")]
        public string DateOfDispatchString { get; set; } = string.Empty;
        public DateOnly DateOfDispatch => DateOnly.ParseExact(DateOfDispatchString, NESTE_DATE_FORMAT);
        [Column(Letter = "O")]
        public string PoSNumber { get; set; } = string.Empty;
        [Column(Letter = "P")]
        public string PoSTicketNumber { get; set; } = string.Empty;
        [Column(Letter = "H")]
        public string Product { get; set; } = string.Empty;
        [Column(Letter = "R")]
        public string RawMaterial { get; set; } = string.Empty;
        [Column(Letter = "S")]
        public string CountryOfOrigin { get; set; } = string.Empty;
        [Column(Letter = "T")]
        public string ProductionCountry { get; set; } = string.Empty;
        [Column(Letter = "U")]
        public string CertificationSystem { get; set; } = string.Empty;
        [Column(Letter = "V")]
        public string SupplierCertificateNumber { get; set; } = string.Empty;
        [Column(Letter = "AB")]
        public decimal QuantityInLitres { get; set; }
        [Column(Letter = "AC")]
        public decimal GHGEmissionSaving { get; set; }
        [Column(Letter = "AD")]
        public decimal GHGEee { get; set; }
        [Column(Letter = "AE")]
        public decimal GHGEccr { get; set; }
        [Column(Letter = "AF")]
        public decimal GHGEccs { get; set; }
        [Column(Letter = "AG")]
        public decimal GHGEsca { get; set; }
        [Column(Letter = "AH")]
        public decimal GHGDistribution { get; set; }
        [Column(Letter = "AI")]
        public decimal GHGTransport { get; set; }
        [Column(Letter = "AJ")]
        public decimal GHGEp { get; set; }
        [Column(Letter = "AL")]
        public decimal GHGEec { get; set; }
        [Column(Letter = "AM")]
        public decimal GHGgCO2eqMJ { get; set; }

        [Column(Letter = "Z")]
        public decimal BioAllocQty { get; set; }
        [Column(Letter = "AA")]
        public decimal BioQuantityNL { get; set; }

        public int RowNumber { get; set; }

        public IncomingDeclaration ToIncomingDeclaration(IReadOnlyList<RawMaterialTranslation> rawMaterialTranslations, IReadOnlyList<ProductTranslation> productTranslations)
        {
            var rawMaterialValue = rawMaterialTranslations.FirstOrDefault(c => c.RawMaterialVariants.Any(desc => StringComparer.OrdinalIgnoreCase.Equals(desc.Value, RawMaterial)))?.RawMaterialStandard.Value;
            var rawMaterial = !string.IsNullOrWhiteSpace(rawMaterialValue) ? Domain.Incoming.RawMaterial.Create(rawMaterialValue) : Domain.Incoming.RawMaterial.Create(RawMaterial);

            var productValue = productTranslations.FirstOrDefault(c => c.ProductVariants.Any(desc => StringComparer.OrdinalIgnoreCase.Equals(desc.Value, Product)))?.ProductStandard.Value;
            var product = !string.IsNullOrWhiteSpace(productValue) ? Domain.Incoming.Product.Create(productValue) : Domain.Incoming.Product.Create(Product);

            return IncomingDeclaration.Create(IncomingDeclarationId.Create(Guid.NewGuid()), Domain.Incoming.Company.Create(Company), Domain.Incoming.Country.Create(Country), product,
           Domain.Incoming.DateOfDispatch.Create(DateOfDispatch), Domain.Incoming.Supplier.Create(Supplier), Domain.Incoming.CertificationSystem.Create(CertificationSystem), Domain.Incoming.SupplierCertificateNumber.Create(SupplierCertificateNumber),
           PosNumber.Create($"{PoSNumber}-{PoSTicketNumber}"), DateOfIssuance.Create(DateOnly.MinValue), Domain.Incoming.PlaceOfDispatch.Create(PlaceOfDispatch),
           Domain.Incoming.ProductionCountry.Create(ProductionCountry), DateOfInstallation.Create(DateTime.MinValue.ToString(CultureInfo.InvariantCulture)), TypeOfProduct.Create(Product), rawMaterial,
           AdditionalInformation.Create(string.Empty), // Missing
           Domain.Incoming.CountryOfOrigin.Create(CountryOfOrigin), Quantity.Create(QuantityInLitres), NESTE_QUANTITY_UOM,
           EnergyContentMJ.Create(0), // Missing
           EnergyQuantityGJ.Create(0), // Missing              
           ComplianceWithSustainabilityCriteria.Create(false), // Missing
           ComplianceWithEuRedMaterialCriteria.Create(false), // Missing
           ComplianceWithIsccMaterialCriteria.Create(false), // Missing
           ChainOfCustodyOption.Create(string.Empty), //Missing 
           CultivatedAsIntermediateCrop.Create(false), // Missing
           FulfillsMeasuresForLowILUCRiskFeedstocks.Create(false), // Missing
           MeetsDefinitionOfWasteOrResidue.Create(false), // Missing
           NUTS2Region.Create(string.Empty), // Missing
           TotalDefaultValueAccordingToRED2.Create(false), // Missing
           Domain.Incoming.GHGEec.Create(GHGEec),
           GHGEl.Create(0), // Missing
           Domain.Incoming.GHGEp.Create(GHGEp),
           GHGEtd.Create(GHGDistribution + GHGTransport), // GHG Etd.
           GHGEu.Create(0), // Missing
           Domain.Incoming.GHGEsca.Create(GHGEsca), Domain.Incoming.GHGEccs.Create(GHGEccs), Domain.Incoming.GHGEccr.Create(GHGEccr), Domain.Incoming.GHGEee.Create(GHGEee),
           GHGgCO2eqPerMJ.Create(GHGgCO2eqMJ),
           FossilFuelComparatorgCO2eqPerMJ.Create(94), // Todo: This should changes based on the product. 
           Domain.Incoming.GHGEmissionSaving.Create(GHGEmissionSaving), DeclarationRowNumber.Create(RowNumber),
           IncomingDeclarationUploadId.Empty(),
           IncomingDeclarationState.Temporary,
           SourceFormatPropertyBag.Create($"{BioAllocQty}:{BioQuantityNL}"));
        }
    }
}
