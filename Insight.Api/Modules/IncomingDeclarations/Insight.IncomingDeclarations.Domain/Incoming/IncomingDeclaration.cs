using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using System.Globalization;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class IncomingDeclaration : Entity
    {
        public IncomingDeclarationId IncomingDeclarationId { get; private set; }
        public Company Company { get; private set; }
        public Country Country { get; private set; }
        public Product Product { get; private set; }
        public DateOfDispatch DateOfDispatch { get; private set; }
        public Supplier Supplier { get; private set; }
        public CertificationSystem CertificationSystem { get; private set; }
        public SupplierCertificateNumber SupplierCertificateNumber { get; private set; }
        public PosNumber PosNumber { get; private set; }
        public DateOfIssuance DateOfIssuance { get; private set; }
        public PlaceOfDispatch PlaceOfDispatch { get; private set; }
        public ProductionCountry ProductionCountry { get; private set; }
        public DateOfInstallation DateOfInstallation { get; private set; }
        public TypeOfProduct TypeOfProduct { get; private set; }
        public RawMaterial RawMaterial { get; private set; }
        public AdditionalInformation AdditionalInformation { get; private set; }
        public CountryOfOrigin CountryOfOrigin { get; private set; }
        public Quantity Quantity { get; private set; }
        public UnitOfMeasurement UnitOfMeasurement { get; private set; }
        public EnergyContentMJ EnergyContentMJ { get; private set; }
        public EnergyQuantityGJ EnergyQuantityGJ { get; private set; }
        public ComplianceWithSustainabilityCriteria ComplianceWithSustainabilityCriteria { get; private set; }
        public ComplianceWithEuRedMaterialCriteria ComplianceWithEuRedMaterialCriteria { get; private set; }
        public ComplianceWithIsccMaterialCriteria ComplianceWithIsccMaterialCriteria { get; private set; }
        public ChainOfCustodyOption ChainOfCustodyOption { get; private set; }
        public CultivatedAsIntermediateCrop CultivatedAsIntermediateCrop { get; private set; }
        public FulfillsMeasuresForLowILUCRiskFeedstocks FulfillsMeasuresForLowILUCRiskFeedstocks { get; private set; }
        public MeetsDefinitionOfWasteOrResidue MeetsDefinitionOfWasteOrResidue { get; private set; }
        public NUTS2Region SpecifyNUTS2Region { get; private set; }
        public TotalDefaultValueAccordingToRED2 TotalDefaultValueAccordingToREDII { get; private set; }
        public GHGEec GHGEec { get; private set; }
        public GHGEl GHGEl { get; private set; }
        public GHGEp GHGEp { get; private set; }
        public GHGEtd GHGEtd { get; private set; }
        public GHGEu GHGEu { get; private set; }
        public GHGEsca GHGEsca { get; private set; }
        public GHGEccs GHGEccs { get; private set; }
        public GHGEccr GHGEccr { get; private set; }
        public GHGEee GHGEee { get; private set; }
        public GHGgCO2eqPerMJ GHGgCO2EqPerMJ { get; private set; }
        public FossilFuelComparatorgCO2eqPerMJ FossilFuelComparatorgCO2EqPerMJ { get; private set; }
        public GHGEmissionSaving GhgEmissionSaving { get; private set; }
        public DeclarationRowNumber DeclarationRowNumber { get; private set; }
        public IncomingDeclarationUploadId IncomingDeclarationUploadId { get; private set; }
        public IncomingDeclarationState IncomingDeclarationState { get; private set; }
        public Allocations Allocations { get; private set; } = Allocations.Empty();
        public decimal RemainingVolume => Quantity.Value - Allocations.TotalAllocatedVolume;
        public SourceFormatPropertyBag SourceFormatPropertyBag { get; private set; } = SourceFormatPropertyBag.Empty();

        public string ItemHash => HashHelpers.GetHashCode(Country.Value, Product.Value, DateOfDispatch.Value, Supplier.Value, CertificationSystem.Value, SupplierCertificateNumber.Value, PosNumber.Value, DateOfIssuance.Value,
            ProductionCountry.Value, TypeOfProduct.Value, RawMaterial.Value, CountryOfOrigin.Value, Quantity.Value.ToString("F", CultureInfo.InvariantCulture), UnitOfMeasurement, EnergyContentMJ.Value.ToString("F", CultureInfo.InvariantCulture), EnergyQuantityGJ.Value.ToString("F", CultureInfo.InvariantCulture), ComplianceWithSustainabilityCriteria.Value,
            CultivatedAsIntermediateCrop.Value, FulfillsMeasuresForLowILUCRiskFeedstocks.Value, MeetsDefinitionOfWasteOrResidue.Value, SpecifyNUTS2Region.Value, TotalDefaultValueAccordingToREDII.Value, GHGEec.Value.ToString("F", CultureInfo.InvariantCulture), GHGEl.Value.ToString("F", CultureInfo.InvariantCulture), ChainOfCustodyOption.Value, ComplianceWithEuRedMaterialCriteria.Value, ComplianceWithIsccMaterialCriteria.Value,
            GHGEp.Value.ToString("F", CultureInfo.InvariantCulture), GHGEtd.Value.ToString("F", CultureInfo.InvariantCulture), GHGEu.Value.ToString("F", CultureInfo.InvariantCulture), GHGEsca.Value.ToString("F", CultureInfo.InvariantCulture), GHGEccs.Value.ToString("F", CultureInfo.InvariantCulture), GHGEccr.Value.ToString("F", CultureInfo.InvariantCulture),
            GHGEee.Value.ToString("F", CultureInfo.InvariantCulture), GHGgCO2EqPerMJ.Value.ToString("F", CultureInfo.InvariantCulture), FossilFuelComparatorgCO2EqPerMJ.Value.ToString("F", CultureInfo.InvariantCulture), GhgEmissionSaving.Value.ToString("F", CultureInfo.InvariantCulture), SourceFormatPropertyBag.Value);
        private IncomingDeclaration()
        {
            IncomingDeclarationId = IncomingDeclarationId.Empty();
            Id = IncomingDeclarationId.Value;
            Company = Company.Empty();
            Country = Country.Empty();
            Product = Product.Empty();
            DateOfDispatch = DateOfDispatch.Empty();
            Supplier = Supplier.Empty();
            CertificationSystem = CertificationSystem.Empty();
            SupplierCertificateNumber = SupplierCertificateNumber.Empty();
            PosNumber = PosNumber.Empty();
            DateOfIssuance = DateOfIssuance.Empty();
            PlaceOfDispatch = PlaceOfDispatch.Empty();
            ProductionCountry = ProductionCountry.Empty();
            DateOfInstallation = DateOfInstallation.Empty();
            TypeOfProduct = TypeOfProduct.Empty();
            RawMaterial = RawMaterial.Empty();
            AdditionalInformation = AdditionalInformation.Empty();
            CountryOfOrigin = CountryOfOrigin.Empty();
            Quantity = Quantity.Empty();
            UnitOfMeasurement = UnitOfMeasurement.Litres;
            EnergyContentMJ = EnergyContentMJ.Empty();
            EnergyQuantityGJ = EnergyQuantityGJ.Empty();
            ComplianceWithSustainabilityCriteria = ComplianceWithSustainabilityCriteria.None();
            ComplianceWithEuRedMaterialCriteria = ComplianceWithEuRedMaterialCriteria.None();
            ComplianceWithIsccMaterialCriteria = ComplianceWithIsccMaterialCriteria.None();
            ChainOfCustodyOption = Incoming.ChainOfCustodyOption.Empty();
            CultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop.None();
            FulfillsMeasuresForLowILUCRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks.None();
            MeetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue.None();
            SpecifyNUTS2Region = NUTS2Region.Empty();
            TotalDefaultValueAccordingToREDII = TotalDefaultValueAccordingToRED2.None();
            GHGEec = GHGEec.Empty();
            GHGEl = GHGEl.Empty();
            GHGEp = GHGEp.Empty();
            GHGEtd = GHGEtd.Empty();
            GHGEu = GHGEu.Empty();
            GHGEsca = GHGEsca.Empty();
            GHGEccs = GHGEccs.Empty();
            GHGEccr = GHGEccr.Empty();
            GHGEee = GHGEee.Empty();
            GHGgCO2EqPerMJ = GHGgCO2eqPerMJ.Empty();
            FossilFuelComparatorgCO2EqPerMJ = FossilFuelComparatorgCO2eqPerMJ.Empty();
            GhgEmissionSaving = GHGEmissionSaving.Empty();
            DeclarationRowNumber = DeclarationRowNumber.Empty();
            IncomingDeclarationUploadId = IncomingDeclarationUploadId.Empty();
            IncomingDeclarationState = IncomingDeclarationState.Temporary;
        }

        private IncomingDeclaration(
            IncomingDeclarationId incomingDeclarationId,
            Company company,
            Country country,
            Product product,
            DateOfDispatch dateOfDispatch,
            Supplier supplier,
            CertificationSystem certificationSystem,
            SupplierCertificateNumber supplierCertificateNumber,
            PosNumber posNumber,
            DateOfIssuance dateOfIssuance,
            PlaceOfDispatch placeOfDispatch,
            ProductionCountry productionCountry,
            DateOfInstallation dateOfInstallation,
            TypeOfProduct typeOfProduct,
            RawMaterial rawMaterial,
            AdditionalInformation additionalInformation,
            CountryOfOrigin countryOfOrigin,
            Quantity quantity,
            UnitOfMeasurement unitOfMeasurement,
            EnergyContentMJ energyContentMJ,
            EnergyQuantityGJ energyQuantityGJ,
            ComplianceWithSustainabilityCriteria complianceWithSustainabilityCriteria,
            ComplianceWithIsccMaterialCriteria complianceWithIsccMaterialCriteria,
            ComplianceWithEuRedMaterialCriteria complianceWithEuRedMaterialCriteria,
            ChainOfCustodyOption chainOfCustodyOption,
            CultivatedAsIntermediateCrop cultivatedAsIntermediateCrop,
            FulfillsMeasuresForLowILUCRiskFeedstocks fulfillsMeasuresForLowILUCRiskFeedstocks,
            MeetsDefinitionOfWasteOrResidue meetsDefinitionOfWasteOrResidue,
            NUTS2Region specifyNUTS2Region,
            TotalDefaultValueAccordingToRED2 totalDefaultValueAccordingToREDII,
            GHGEec gHGEec,
            GHGEl gHGEl,
            GHGEp gHGEp,
            GHGEtd gHGEtd,
            GHGEu gHGEu,
            GHGEsca gHGEsca,
            GHGEccs gHGEccs,
            GHGEccr gHGEccr,
            GHGEee gHGEee,
            GHGgCO2eqPerMJ gHGgCO2eqPerMJ,
            FossilFuelComparatorgCO2eqPerMJ fossilFuelComparatorgCO2eqPerMJ,
            GHGEmissionSaving gHGEmissionSaving,
            DeclarationRowNumber declarationRowNumber,
            IncomingDeclarationUploadId incomingDeclarationUploadId,
            IncomingDeclarationState incomingDeclarationState,
            SourceFormatPropertyBag sourceFormatPropertyBag)
        {
            IncomingDeclarationId = incomingDeclarationId;
            Id = IncomingDeclarationId.Value;
            Company = company;
            Country = country;
            Product = product;
            DateOfDispatch = dateOfDispatch;
            Supplier = supplier;
            CertificationSystem = certificationSystem;
            SupplierCertificateNumber = supplierCertificateNumber;
            PosNumber = posNumber;
            DateOfIssuance = dateOfIssuance;
            PlaceOfDispatch = placeOfDispatch;
            ProductionCountry = productionCountry;
            DateOfInstallation = dateOfInstallation;
            TypeOfProduct = typeOfProduct;
            RawMaterial = rawMaterial;
            AdditionalInformation = additionalInformation;
            CountryOfOrigin = countryOfOrigin;
            Quantity = quantity;
            UnitOfMeasurement = unitOfMeasurement;
            EnergyContentMJ = energyContentMJ;
            EnergyQuantityGJ = energyQuantityGJ;
            ComplianceWithSustainabilityCriteria = complianceWithSustainabilityCriteria;
            ComplianceWithEuRedMaterialCriteria = complianceWithEuRedMaterialCriteria;
            ComplianceWithIsccMaterialCriteria = complianceWithIsccMaterialCriteria;
            ChainOfCustodyOption = chainOfCustodyOption;
            CultivatedAsIntermediateCrop = cultivatedAsIntermediateCrop;
            FulfillsMeasuresForLowILUCRiskFeedstocks = fulfillsMeasuresForLowILUCRiskFeedstocks;
            MeetsDefinitionOfWasteOrResidue = meetsDefinitionOfWasteOrResidue;
            SpecifyNUTS2Region = specifyNUTS2Region;
            TotalDefaultValueAccordingToREDII = totalDefaultValueAccordingToREDII;
            GHGEec = gHGEec;
            GHGEl = gHGEl;
            GHGEp = gHGEp;
            GHGEtd = gHGEtd;
            GHGEu = gHGEu;
            GHGEsca = gHGEsca;
            GHGEccs = gHGEccs;
            GHGEccr = gHGEccr;
            GHGEee = gHGEee;
            GHGgCO2EqPerMJ = gHGgCO2eqPerMJ;
            FossilFuelComparatorgCO2EqPerMJ = fossilFuelComparatorgCO2eqPerMJ;
            GhgEmissionSaving = gHGEmissionSaving;
            DeclarationRowNumber = declarationRowNumber;
            IncomingDeclarationUploadId = incomingDeclarationUploadId;
            IncomingDeclarationState = incomingDeclarationState;
            SourceFormatPropertyBag = sourceFormatPropertyBag;
        }

        public void SetIncomingDeclarationUploadId(IncomingDeclarationUploadId incomingDeclarationUploadId)
        {
            IncomingDeclarationUploadId = incomingDeclarationUploadId;
        }

        public static IncomingDeclaration Create(
            IncomingDeclarationId incomingDeclarationId,
            Company company,
            Country country,
            Product product,
            DateOfDispatch dateOfDispatch,
            Supplier supplier,
            CertificationSystem certificationSystem,
            SupplierCertificateNumber supplierCertificateNumber,
            PosNumber poSNumber,
            DateOfIssuance dateOfIssuance,
            PlaceOfDispatch placeOfDispatch,
            ProductionCountry productionCountry,
            DateOfInstallation dateOfInstallation,
            TypeOfProduct typeOfProduct,
            RawMaterial rawMaterial,
            AdditionalInformation additionalInformation,
            CountryOfOrigin countryOfOrigin,
            Quantity quantity,
            UnitOfMeasurement unitOfMeasurement,
            EnergyContentMJ energyContentMJ,
            EnergyQuantityGJ energyQuantityGJ,
            ComplianceWithSustainabilityCriteria complianceWithSustainabilityCriteria,
            ComplianceWithEuRedMaterialCriteria complianceWithEuRedMaterialCriteria,
            ComplianceWithIsccMaterialCriteria complianceWithIsccMaterialCriteria,
            ChainOfCustodyOption chainOfCustodyOption,
            CultivatedAsIntermediateCrop cultivatedAsIntermediateCrop,
            FulfillsMeasuresForLowILUCRiskFeedstocks fulfillsMeasuresForLowILUCRiskFeedstocks,
            MeetsDefinitionOfWasteOrResidue meetsDefinitionOfWasteOrResidue,
            NUTS2Region specifyNUTS2Region,
            TotalDefaultValueAccordingToRED2 totalDefaultValueAccordingToREDII,
            GHGEec gHGEec,
            GHGEl gHGEl,
            GHGEp gHGEp,
            GHGEtd gHGEtd,
            GHGEu gHGEu,
            GHGEsca gHGEsca,
            GHGEccs gHGEccs,
            GHGEccr gHGEccr,
            GHGEee gHGEee,
            GHGgCO2eqPerMJ gHGgCO2eqPerMJ,
            FossilFuelComparatorgCO2eqPerMJ fossilFuelComparatorgCO2eqPerMJ,
            GHGEmissionSaving gHGEmissionSaving,
            DeclarationRowNumber declarationRowNumber,
            IncomingDeclarationUploadId incomingDeclarationUploadId,
            IncomingDeclarationState incomingDeclarationState,
            SourceFormatPropertyBag sourceFormatPropertyBag)
        {
            return new IncomingDeclaration(
                incomingDeclarationId,
                company,
                country,
                product,
                dateOfDispatch,
                supplier,
                certificationSystem,
                supplierCertificateNumber,
                poSNumber,
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
                energyContentMJ,
                energyQuantityGJ,
                complianceWithSustainabilityCriteria,
                complianceWithIsccMaterialCriteria,
                complianceWithEuRedMaterialCriteria,
                chainOfCustodyOption,
                cultivatedAsIntermediateCrop,
                fulfillsMeasuresForLowILUCRiskFeedstocks,
                meetsDefinitionOfWasteOrResidue,
                specifyNUTS2Region,
                totalDefaultValueAccordingToREDII,
                gHGEec,
                gHGEl,
                gHGEp,
                gHGEtd,
                gHGEu,
                gHGEsca,
                gHGEccs,
                gHGEccr,
                gHGEee,
                gHGgCO2eqPerMJ,
                fossilFuelComparatorgCO2eqPerMJ,
                gHGEmissionSaving,
                declarationRowNumber,
                incomingDeclarationUploadId,
                incomingDeclarationState,
                sourceFormatPropertyBag);
        }

        public static IncomingDeclaration Empty()
        {
            return new IncomingDeclaration();
        }

        public void SetIncomingDeclarationState(IncomingDeclarationState newState)
        {
            IncomingDeclarationState = newState;
        }

        private void SetCompany(Company company)
        {
            Company = company;
        }

        private void SetCountry(Country country)
        {
            Country = country;
        }

        private void SetProduct(Product product)
        {
            Product = product;
        }

        private void SetSupplier(Supplier supplier)
        {
            Supplier = supplier;
        }

        private void SetRawMaterial(RawMaterial rawMaterial)
        {
            RawMaterial = rawMaterial;
        }

        private void SetPosNumber(PosNumber posNumber)
        {
            PosNumber = posNumber;
        }

        private void SetCountryOfOrigin(CountryOfOrigin countryOfOrigin)
        {
            CountryOfOrigin = countryOfOrigin;
        }
        
        public void SetUpdatedIncomingDeclaration(IncomingDeclarationUpdateParameters incomingDeclarationUpdateParameters)
        {
            SetCountryOfOrigin(incomingDeclarationUpdateParameters.CountryOfOrigin);
            SetCompany(incomingDeclarationUpdateParameters.Company);
            SetSupplier(incomingDeclarationUpdateParameters.Supplier);
            SetCountry(incomingDeclarationUpdateParameters.Country);
            SetProduct(incomingDeclarationUpdateParameters.Product);
            SetPosNumber(incomingDeclarationUpdateParameters.PosNumber);
            SetRawMaterial(incomingDeclarationUpdateParameters.RawMaterial);
        }

        public bool AddAllocation(Guid allocationId, decimal volume)
        {
            
            if(volume > RemainingVolume)
            {
                return false;                
            }

            return Allocations.Value.TryAdd(allocationId, volume);
        }
    }
}