using AutoFixture;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        internal static IncomingDeclarationUpdateParameters IncomingDeclarationUpdateParameters()
        {
            var incomingDeclarationId = IncomingDeclarationId();
            var company = Company();
            var country = Country();
            var product = Product();
            var dateOfDispatch = DateOfDispatch();
            var supplier = Supplier();
            var certificationSystem = CertificationSystem();
            var supplierCertificateNumber = SupplierCertificateNumber();
            var posNumber = PosNumber();
            var dateOfIssuance = DateOfIssuance();
            var placeOfDispatch = PlaceOfDispatch();
            var productionCountry = ProductionCountry();
            var dateOfInstallation = DateOfInstallation();
            var typeOfProduct = TypeOfProduct();
            var rawMaterial = RawMaterial();
            var additionalInformation = AdditionalInformation();
            var countryOfOrigin = CountryOfOrigin();
            var quantity = Quantity();
            var unitOfMeasurement = UnitOfMeasurement();
            var energyContentMj = EnergyContentMJ();
            var energyQuantityGj = EnergyQuantityGJ();
            var complianceWithSustainabilityCriteria = ComplianceWithSustainabilityCriteria();
            var cultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop();
            var fulfillsMeasuresForLowIlucRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks();
            var meetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue();
            var nuts2Region = NUTS2Region();
            var totalDefaultValueAccordingToRed2 = TotalDefaultValueAccordingToREDII();
            var ghgEec = GHGEec();
            var ghgEl = GHGEl();
            var ghgEp = GHGEp();
            var ghgEtd = GHGEtd();
            var ghgEu = GHGEu();
            var ghgEsca = GHGEsca();
            var ghgEccs = GHGEccs();
            var ghgEccr = GHGEccr();
            var ghgEee = GHGEee();
            var ghGgCo2EqPerMj = GHGgCO2eqPerMJ();
            var fossilFuelComparatorgCo2EqPerMj = FossilFuelComparatorgCO2eqPerMJ();
            var ghgEmissionSaving = GHGEmissionSaving();
            var rowNumber = RowNumber();
            var incomingDeclarationUploadId = IncomingDeclarationUploadId();

            return IncomingDeclarations.Domain.Incoming.IncomingDeclarationUpdateParameters.Create(
                company,
                country,
                product,
                supplier,
                rawMaterial,
                posNumber,
                countryOfOrigin,
                dateOfDispatch,
                certificationSystem,
                supplierCertificateNumber,
                dateOfIssuance,
                placeOfDispatch,
                productionCountry,
                dateOfInstallation,
                typeOfProduct,
                additionalInformation,
                quantity,
                unitOfMeasurement,
                energyContentMj,
                energyQuantityGj,
                complianceWithSustainabilityCriteria,
                cultivatedAsIntermediateCrop,
                fulfillsMeasuresForLowIlucRiskFeedstocks,
                meetsDefinitionOfWasteOrResidue,
                nuts2Region,
                totalDefaultValueAccordingToRed2,
                ghgEec,
                ghgEl,
                ghgEp,
                ghgEtd,
                ghgEu,
                ghgEsca,
                ghgEccs,
                ghgEccr,
                ghgEee,
                ghGgCo2EqPerMj,
                fossilFuelComparatorgCo2EqPerMj,
                ghgEmissionSaving,
                rowNumber,
                incomingDeclarationUploadId);
        }

        internal static IncomingDeclaration IncomingDeclaration(DateOfDispatch dateOfDispatch, Product product, Company company, Supplier supplier)
        {
            var incomingDeclarationId = IncomingDeclarationId();
            var country = Country();
            var certificationSystem = CertificationSystem();
            var supplierCertificateNumber = SupplierCertificateNumber();
            var posNumber = PosNumber();
            var dateOfIssuance = DateOfIssuance();
            var placeOfDispatch = PlaceOfDispatch();
            var productionCountry = ProductionCountry();
            var dateOfInstallation = DateOfInstallation();
            var typeOfProduct = TypeOfProduct();
            var rawMaterial = RawMaterial();
            var additionalInformation = AdditionalInformation();
            var countryOfOrigin = CountryOfOrigin();
            var quantity = Quantity();
            var unitOfMeasurement = UnitOfMeasurement();
            var energyContentMj = EnergyContentMJ();
            var energyQuantityGj = EnergyQuantityGJ();
            var complianceWithSustainabilityCriteria = ComplianceWithSustainabilityCriteria();
            var complianceWithEuRedMaterialCriteria = ComplianceWithEuRedMaterialCriteria();
            var complianceWithIsccMaterialCriteria = ComplianceWithIsccMaterialCriteria();
            var chainOfCustody = ChainOfCustodyOption();
            var cultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop();
            var fulfillsMeasuresForLowIlucRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks();
            var meetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue();
            var nuts2Region = NUTS2Region();
            var totalDefaultValueAccordingToRed2 = TotalDefaultValueAccordingToREDII();
            var ghgEec = GHGEec();
            var ghgEl = GHGEl();
            var ghgEp = GHGEp();
            var ghgEtd = GHGEtd();
            var ghgEu = GHGEu();
            var ghgEsca = GHGEsca();
            var ghgEccs = GHGEccs();
            var ghgEccr = GHGEccr();
            var ghgEee = GHGEee();
            var ghGgCo2EqPerMj = GHGgCO2eqPerMJ();
            var fossilFuelComparatorgCo2EqPerMj = FossilFuelComparatorgCO2eqPerMJ();
            var ghgEmissionSaving = GHGEmissionSaving();
            var rowNumber = RowNumber();
            var incomingDeclarationUploadId = IncomingDeclarationUploadId();
            var sourceFormatPropertyBag = SourceFormatPropertyBag();
            const IncomingDeclarationState incomingDeclarationState = IncomingDeclarationState.Temporary;

            return IncomingDeclarations.Domain.Incoming.IncomingDeclaration.Create(
                incomingDeclarationId,
                company,
                country,
                product,
                dateOfDispatch,
                supplier,
                certificationSystem,
                supplierCertificateNumber,
                posNumber,
                dateOfIssuance,
                placeOfDispatch,
                productionCountry,
                dateOfInstallation,
                typeOfProduct,
                rawMaterial,
                additionalInformation,
                countryOfOrigin,
                quantity,
                unitOfMeasurement,
                energyContentMj,
                energyQuantityGj,
                complianceWithSustainabilityCriteria,
                complianceWithEuRedMaterialCriteria,
                complianceWithIsccMaterialCriteria,
                chainOfCustody,
                cultivatedAsIntermediateCrop,
                fulfillsMeasuresForLowIlucRiskFeedstocks,
                meetsDefinitionOfWasteOrResidue,
                nuts2Region,
                totalDefaultValueAccordingToRed2,
                ghgEec,
                ghgEl,
                ghgEp,
                ghgEtd,
                ghgEu,
                ghgEsca,
                ghgEccs,
                ghgEccr,
                ghgEee,
                ghGgCo2EqPerMj,
                fossilFuelComparatorgCo2EqPerMj,
                ghgEmissionSaving,
                rowNumber,
                incomingDeclarationUploadId,
                incomingDeclarationState,
                sourceFormatPropertyBag);
        }

        internal static IncomingDeclaration IncomingDeclaration(DateOfDispatch dateOfDispatch)
        {
            return IncomingDeclaration(dateOfDispatch, Product(), Company(), Supplier());
        }

        internal static IncomingDeclaration IncomingDeclaration(Product product)
        {
            return IncomingDeclaration(DateOfDispatch(), product, Company(), Supplier());
        }

        internal static IncomingDeclaration IncomingDeclaration(Company company)
        {
            return IncomingDeclaration(DateOfDispatch(), Product(), company, Supplier());
        }

        internal static IncomingDeclaration IncomingDeclaration(Supplier supplier)
        {
            return IncomingDeclaration(DateOfDispatch(), Product(), Company(), supplier);
        }

        internal static IncomingDeclaration IncomingDeclaration()
        {
            return IncomingDeclaration(DateOfDispatch(), Product(), Company(), Supplier());
        }

        internal static IncomingDeclaration IncomingDeclaration(DateOfDispatch dateOfDispatch, Product product, Country country, PlaceOfDispatch placeOfDispatch, GHGEmissionSaving ghgEmissionSaving, Quantity quantity)
        {
            var incomingDeclarationId = IncomingDeclarationId();
            var company = Company();
            var supplier = Supplier();
            var certificationSystem = CertificationSystem();
            var supplierCertificateNumber = SupplierCertificateNumber();
            var posNumber = PosNumber();
            var dateOfIssuance = DateOfIssuance();
            var productionCountry = ProductionCountry();
            var dateOfInstallation = DateOfInstallation();
            var typeOfProduct = TypeOfProduct();
            var rawMaterial = RawMaterial();
            var additionalInformation = AdditionalInformation();
            var countryOfOrigin = CountryOfOrigin();
            var unitOfMeasurement = UnitOfMeasurement();
            var energyContentMj = EnergyContentMJ();
            var energyQuantityGj = EnergyQuantityGJ();
            var complianceWithSustainabilityCriteria = ComplianceWithSustainabilityCriteria();
            var complianceWithEuRedMaterialCriteria = ComplianceWithEuRedMaterialCriteria();
            var complianceWithIsccMaterialCriteria = ComplianceWithIsccMaterialCriteria();
            var chainOfCustodyOption = ChainOfCustodyOption();
            var cultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop();
            var fulfillsMeasuresForLowIlucRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks();
            var meetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue();
            var nuts2Region = NUTS2Region();
            var totalDefaultValueAccordingToRed2 = TotalDefaultValueAccordingToREDII();
            var ghgEec = GHGEec();
            var ghgEl = GHGEl();
            var ghgEp = GHGEp();
            var ghgEtd = GHGEtd();
            var ghgEu = GHGEu();
            var ghgEsca = GHGEsca();
            var ghgEccs = GHGEccs();
            var ghgEccr = GHGEccr();
            var ghgEee = GHGEee();
            var ghGgCo2EqPerMj = GHGgCO2eqPerMJ();
            var fossilFuelComparatorgCo2EqPerMj = FossilFuelComparatorgCO2eqPerMJ();
            var rowNumber = RowNumber();
            var incomingDeclarationUploadId = IncomingDeclarationUploadId();
            var sourceFormatPropertyBag = SourceFormatPropertyBag();
            const IncomingDeclarationState incomingDeclarationState = IncomingDeclarationState.Temporary;

            return IncomingDeclarations.Domain.Incoming.IncomingDeclaration.Create(
                incomingDeclarationId,
                company,
                country,
                product,
                dateOfDispatch,
                supplier,
                certificationSystem,
                supplierCertificateNumber,
                posNumber,
                dateOfIssuance,
                placeOfDispatch,
                productionCountry,
                dateOfInstallation,
                typeOfProduct,
                rawMaterial,
                additionalInformation,
                countryOfOrigin,
                quantity,
                unitOfMeasurement,
                energyContentMj,
                energyQuantityGj,
                complianceWithSustainabilityCriteria,
                complianceWithEuRedMaterialCriteria,
                complianceWithIsccMaterialCriteria,
                chainOfCustodyOption,
                cultivatedAsIntermediateCrop,
                fulfillsMeasuresForLowIlucRiskFeedstocks,
                meetsDefinitionOfWasteOrResidue,
                nuts2Region,
                totalDefaultValueAccordingToRed2,
                ghgEec,
                ghgEl,
                ghgEp,
                ghgEtd,
                ghgEu,
                ghgEsca,
                ghgEccs,
                ghgEccr,
                ghgEee,
                ghGgCo2EqPerMj,
                fossilFuelComparatorgCo2EqPerMj,
                ghgEmissionSaving,
                rowNumber,
                incomingDeclarationUploadId,
                incomingDeclarationState,
                sourceFormatPropertyBag);
        }

        private static SourceFormatPropertyBag SourceFormatPropertyBag()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.SourceFormatPropertyBag.Create(Instance<string>());
        }

        private static IncomingDeclarationId IncomingDeclarationId()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.IncomingDeclarationId.Create(Instance<Guid>());
        }

        private static Company Company()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.Company.Create(Instance<string>());
        }

        private static Country Country()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.Country.Create(Instance<string>());
        }

        private static Product Product()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.Product.Create(Instance<string>());

        }

        private static DateOfDispatch DateOfDispatch()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.DateOfDispatch.Create(DateOnly.MaxValue);
        }

        private static Supplier Supplier()
        {
            return IncomingDeclarations.Domain.Incoming.Supplier.Create(Instance<string>());

        }

        private static CertificationSystem CertificationSystem()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.CertificationSystem.Create(Instance<string>());

        }

        private static SupplierCertificateNumber SupplierCertificateNumber()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.SupplierCertificateNumber.Create(Instance<string>());
        }

        private static PosNumber PosNumber()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.PosNumber.Create(Instance<string>());
        }

        private static DateOfIssuance DateOfIssuance()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.DateOfIssuance.Create(DateOnly.MaxValue);
        }

        private static PlaceOfDispatch PlaceOfDispatch()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.PlaceOfDispatch.Create(Instance<string>());
        }

        private static ProductionCountry ProductionCountry()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.ProductionCountry.Create(Instance<string>());
        }

        private static DateOfInstallation DateOfInstallation()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.DateOfInstallation.Create(Instance<string>());
        }

        private static TypeOfProduct TypeOfProduct()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.TypeOfProduct.Create(Instance<string>());
        }

        private static RawMaterial RawMaterial()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.RawMaterial.Create(Instance<string>());
        }

        private static AdditionalInformation AdditionalInformation()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.AdditionalInformation.Create(Instance<string>());
        }

        private static CountryOfOrigin CountryOfOrigin()
        {
            return  Insight.IncomingDeclarations.Domain.Incoming.CountryOfOrigin.Create(Instance<string>());
        }

        private static Quantity Quantity()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.Quantity.Create(Instance<decimal>());
        }

        private static UnitOfMeasurement UnitOfMeasurement()
        {
            return IncomingDeclarations.Domain.Incoming.UnitOfMeasurement.Litres;
        }

        private static EnergyContentMJ EnergyContentMJ()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.EnergyContentMJ.Create(Instance<decimal>());
        }

        private static EnergyQuantityGJ EnergyQuantityGJ()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.EnergyQuantityGJ.Create(Instance<decimal>());
        }

        private static ComplianceWithSustainabilityCriteria ComplianceWithSustainabilityCriteria()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.ComplianceWithSustainabilityCriteria.Create(Instance<bool>());
        }
        private static ComplianceWithEuRedMaterialCriteria ComplianceWithEuRedMaterialCriteria()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.ComplianceWithEuRedMaterialCriteria.Create(Instance<bool>());
        }
        private static ComplianceWithIsccMaterialCriteria ComplianceWithIsccMaterialCriteria()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.ComplianceWithIsccMaterialCriteria.Create(Instance<bool>());
        }
        private static ChainOfCustodyOption ChainOfCustodyOption()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.ChainOfCustodyOption.Create(Instance<string>());
        }

        private static CultivatedAsIntermediateCrop CultivatedAsIntermediateCrop()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.CultivatedAsIntermediateCrop.Create(Instance<bool>());
        }

        private static FulfillsMeasuresForLowILUCRiskFeedstocks FulfillsMeasuresForLowILUCRiskFeedstocks()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.FulfillsMeasuresForLowILUCRiskFeedstocks.Create(Instance<bool>());
        }

        private static MeetsDefinitionOfWasteOrResidue MeetsDefinitionOfWasteOrResidue()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.MeetsDefinitionOfWasteOrResidue.Create(Instance<bool>());
        }

        private static NUTS2Region NUTS2Region()
        {
            return IncomingDeclarations.Domain.Incoming.NUTS2Region.Create(Instance<string>());
        }

        private static TotalDefaultValueAccordingToRED2 TotalDefaultValueAccordingToREDII()
        {
            return TotalDefaultValueAccordingToRED2.Create(Instance<bool>());
        }

        private static GHGEec GHGEec()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEec.Create(Instance<decimal>());
        }

        private static GHGEl GHGEl()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEl.Create(Instance<decimal>());
        }

        private static GHGEp GHGEp()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEp.Create(Instance<decimal>());
        }

        private static GHGEtd GHGEtd()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEtd.Create(Instance<decimal>());
        }

        private static GHGEu GHGEu()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEu.Create(Instance<decimal>());
        }

        private static GHGEsca GHGEsca()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEsca.Create(Instance<decimal>());
        }

        private static GHGEccs GHGEccs()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEccs.Create(Instance<decimal>());
        }

        private static GHGEccr GHGEccr()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEccr.Create(Instance<decimal>());
        }

        private static GHGEee GHGEee()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEee.Create(Instance<decimal>());
        }

        private static GHGgCO2eqPerMJ GHGgCO2eqPerMJ()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGgCO2eqPerMJ.Create(Instance<decimal>());
        }

        private static FossilFuelComparatorgCO2eqPerMJ FossilFuelComparatorgCO2eqPerMJ()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.FossilFuelComparatorgCO2eqPerMJ.Create(Instance<decimal>());
        }

        private static GHGEmissionSaving GHGEmissionSaving()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.GHGEmissionSaving.Create(Instance<decimal>());
        }

        private static DeclarationRowNumber RowNumber()
        {
            return DeclarationRowNumber.Create(Instance<int>());
        }

        private static IncomingDeclarationUploadId IncomingDeclarationUploadId()
        {
            return Insight.IncomingDeclarations.Domain.Incoming.IncomingDeclarationUploadId.Create(Instance<Guid>());
        }
    }
}
