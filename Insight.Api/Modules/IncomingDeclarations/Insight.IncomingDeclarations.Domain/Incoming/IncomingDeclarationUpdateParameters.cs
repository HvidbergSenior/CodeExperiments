using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming;

public sealed class IncomingDeclarationUpdateParameters : ValueObject
{
    public Company Company { get; private set; }
    public Country Country { get; private set; }
    public Product Product { get; private set; }
    public Supplier Supplier { get; private set; }
    public RawMaterial RawMaterial { get; private set; }
    public PosNumber PosNumber { get; private set; }
    public CountryOfOrigin CountryOfOrigin { get; private set; }
    public IncomingDeclarationState IncomingDeclarationState { get; private set; }
    public DateOfDispatch DateOfDispatch { get; private set; }
    public CertificationSystem CertificationSystem { get; private set; }
    public SupplierCertificateNumber SupplierCertificateNumber { get; private set; }
    public DateOfIssuance DateOfIssuance { get; private set; }
    public PlaceOfDispatch PlaceOfDispatch { get; private set; }
    public ProductionCountry ProductionCountry { get; private set; }
    public DateOfInstallation DateOfInstallation { get; private set; }
    public TypeOfProduct TypeOfProduct { get; private set; }
    public AdditionalInformation AdditionalInformation { get; private set; }
    public Quantity Quantity { get; private set; }
    public UnitOfMeasurement UnitOfMeasurement { get; private set; }
    public EnergyContentMJ EnergyContentMJ { get; private set; }
    public EnergyQuantityGJ EnergyQuantityGJ { get; private set; }
    public ComplianceWithSustainabilityCriteria ComplianceWithSustainabilityCriteria { get; private set; }
    public CultivatedAsIntermediateCrop CultivatedAsIntermediateCrop { get; private set; }
    public FulfillsMeasuresForLowILUCRiskFeedstocks FulfillsMeasuresForLowILUCRiskFeedstocks { get; private set; }
    public MeetsDefinitionOfWasteOrResidue MeetsDefinitionOfWasteOrResidue { get; private set; }
    public NUTS2Region NUTS2Region { get; private set; }
    public TotalDefaultValueAccordingToRED2 TotalDefaultValueAccordingToRED2 { get; private set; }
    public GHGEec GHGEec { get; private set; }
    public GHGEl GHGEl { get; private set; }
    public GHGEp GHGEp { get; private set; }
    public GHGEtd GHGEtd { get; private set; }
    public GHGEu GHGEu { get; private set; }
    public GHGEsca GHGEsca { get; private set; }
    public GHGEccs GHGEccs { get; private set; }
    public GHGEccr GHGEccr { get; private set; }
    public GHGEee GHGEee { get; private set; }
    public GHGgCO2eqPerMJ GHGgCO2eqPerMJ { get; private set; }
    public FossilFuelComparatorgCO2eqPerMJ FossilFuelComparatorgCO2eqPerMJ { get; private set; }
    public GHGEmissionSaving GHGEmissionSaving { get; private set; }
    public DeclarationRowNumber DeclarationRowNumber { get; private set; }
    public IncomingDeclarationUploadId IncomingDeclarationUploadId { get; private set; }

    private IncomingDeclarationUpdateParameters()
    {
        Company = Company.Empty();
        Country = Country.Empty();
        Product = Product.Empty();
        Supplier = Supplier.Empty();
        RawMaterial = RawMaterial.Empty();
        PosNumber = PosNumber.Empty();
        CountryOfOrigin = CountryOfOrigin.Empty();
        DateOfDispatch = DateOfDispatch.Empty();
        CertificationSystem = CertificationSystem.Empty();
        SupplierCertificateNumber = SupplierCertificateNumber.Empty();
        DateOfIssuance = DateOfIssuance.Empty();
        PlaceOfDispatch = PlaceOfDispatch.Empty();
        ProductionCountry = ProductionCountry.Empty();
        DateOfInstallation = DateOfInstallation.Empty();
        TypeOfProduct = TypeOfProduct.Empty();
        AdditionalInformation = AdditionalInformation.Empty();
        Quantity = Quantity.Empty();
        UnitOfMeasurement = UnitOfMeasurement.Kilograms;
        EnergyContentMJ = EnergyContentMJ.Empty();
        EnergyQuantityGJ = EnergyQuantityGJ.Empty();
        ComplianceWithSustainabilityCriteria = ComplianceWithSustainabilityCriteria.None();
        CultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop.None();
        FulfillsMeasuresForLowILUCRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks.None();
        MeetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue.None();
        NUTS2Region = NUTS2Region.Empty(); 
        TotalDefaultValueAccordingToRED2 = TotalDefaultValueAccordingToRED2.None();
        GHGEec = GHGEec.Empty();
        GHGEl = GHGEl.Empty();
        GHGEp = GHGEp.Empty();
        GHGEtd = GHGEtd.Empty();
        GHGEu = GHGEu.Empty();
        GHGEsca = GHGEsca.Empty();
        GHGEccs = GHGEccs.Empty();
        GHGEccr = GHGEccr.Empty();
        GHGEee = GHGEee.Empty();
        GHGgCO2eqPerMJ = GHGgCO2eqPerMJ.Empty();
        FossilFuelComparatorgCO2eqPerMJ = FossilFuelComparatorgCO2eqPerMJ.Empty();
        GHGEmissionSaving = GHGEmissionSaving.Empty();
        DeclarationRowNumber = DeclarationRowNumber.Empty();
        IncomingDeclarationUploadId = IncomingDeclarationUploadId.Empty();
    }

    private IncomingDeclarationUpdateParameters(
        Company company,
        Country country,
        Product product,
        Supplier supplier,
        RawMaterial rawMaterial,
        PosNumber posNumber,
        CountryOfOrigin countryOfOrigin,
        DateOfDispatch dateOfDispatch,
        CertificationSystem certificationSystem,
        SupplierCertificateNumber supplierCertificateNumber,
        DateOfIssuance dateOfIssuance,
        PlaceOfDispatch placeOfDispatch,
        ProductionCountry productionCountry,
        DateOfInstallation dateOfInstallation,
        TypeOfProduct typeOfProduct,
        AdditionalInformation additionalInformation,
        Quantity quantity,
        UnitOfMeasurement unitOfMeasurement,
        EnergyContentMJ energyContentMj,
        EnergyQuantityGJ energyQuantityGj,
        ComplianceWithSustainabilityCriteria complianceWithSustainabilityCriteria,
        CultivatedAsIntermediateCrop cultivatedAsIntermediateCrop,
        FulfillsMeasuresForLowILUCRiskFeedstocks fulfillsMeasuresForLowIlucRiskFeedstocks,
        MeetsDefinitionOfWasteOrResidue meetsDefinitionOfWasteOrResidue,
        NUTS2Region specifyNUTS2Region,
        TotalDefaultValueAccordingToRED2 totalDefaultValueAccordingToREDII, 
        GHGEec ghgEec,
        GHGEl ghgEl,
        GHGEp ghgEp,
        GHGEtd ghgEtd,
        GHGEu ghgEu,
        GHGEsca ghgEsca,
        GHGEccs ghgEccs,
        GHGEccr ghgEccr,
        GHGEee ghgEee,
        GHGgCO2eqPerMJ gHGgCO2EqPerMJ,
        FossilFuelComparatorgCO2eqPerMJ fossilFuelComparatorgCO2EqPerMJ,
        GHGEmissionSaving ghgEmissionSaving,
        DeclarationRowNumber declarationRowNumber,
        IncomingDeclarationUploadId incomingDeclarationUploadId)
    {
        Company = company;
        Country = country;
        Product = product;
        Supplier = supplier;
        RawMaterial = rawMaterial;
        PosNumber = posNumber;
        CountryOfOrigin = countryOfOrigin;
        DateOfDispatch = dateOfDispatch;
        CertificationSystem = certificationSystem;
        SupplierCertificateNumber = supplierCertificateNumber;
        DateOfIssuance = dateOfIssuance;
        PlaceOfDispatch = placeOfDispatch;
        ProductionCountry = productionCountry;
        DateOfInstallation = dateOfInstallation;
        TypeOfProduct = typeOfProduct;
        AdditionalInformation = additionalInformation;
        Quantity = quantity;
        UnitOfMeasurement = unitOfMeasurement;
        EnergyContentMJ = energyContentMj;
        EnergyQuantityGJ = energyQuantityGj;
        ComplianceWithSustainabilityCriteria = complianceWithSustainabilityCriteria;
        CultivatedAsIntermediateCrop = cultivatedAsIntermediateCrop;
        FulfillsMeasuresForLowILUCRiskFeedstocks = fulfillsMeasuresForLowIlucRiskFeedstocks;
        MeetsDefinitionOfWasteOrResidue = meetsDefinitionOfWasteOrResidue;
        NUTS2Region = specifyNUTS2Region;
        TotalDefaultValueAccordingToRED2 = totalDefaultValueAccordingToREDII;
        GHGEec = ghgEec;
        GHGEl = ghgEl;
        GHGEp = ghgEp;
        GHGEtd = ghgEtd;
        GHGEu = ghgEu;
        GHGEsca = ghgEsca;
        GHGEccs = ghgEccs;
        GHGEccr = ghgEccr;
        GHGEee = ghgEee;
        GHGgCO2eqPerMJ = gHGgCO2EqPerMJ;
        FossilFuelComparatorgCO2eqPerMJ = fossilFuelComparatorgCO2EqPerMJ;
        GHGEmissionSaving = ghgEmissionSaving;
        DeclarationRowNumber = declarationRowNumber;
        IncomingDeclarationUploadId = incomingDeclarationUploadId;
    }

    public static IncomingDeclarationUpdateParameters Create(
        Company company,
        Country country,
        Product product,
        Supplier supplier,
        RawMaterial rawMaterial,
        PosNumber posNumber,
        CountryOfOrigin countryOfOrigin,
        DateOfDispatch dateOfDispatch,
        CertificationSystem certificationSystem,
        SupplierCertificateNumber supplierCertificateNumber,
        DateOfIssuance dateOfIssuance,
        PlaceOfDispatch placeOfDispatch,
        ProductionCountry productionCountry,
        DateOfInstallation dateOfInstallation,
        TypeOfProduct typeOfProduct,
        AdditionalInformation additionalInformation,
        Quantity quantity,
        UnitOfMeasurement unitOfMeasurement,
        EnergyContentMJ energyContentMj,
        EnergyQuantityGJ energyQuantityGj,
        ComplianceWithSustainabilityCriteria complianceWithSustainabilityCriteria,
        CultivatedAsIntermediateCrop cultivatedAsIntermediateCrop,
        FulfillsMeasuresForLowILUCRiskFeedstocks fulfillsMeasuresForLowIlucRiskFeedstocks,
        MeetsDefinitionOfWasteOrResidue meetsDefinitionOfWasteOrResidue, 
        NUTS2Region nUTS2Region,
        TotalDefaultValueAccordingToRED2 totalDefaultValueAccordingToRED2,
        GHGEec ghgEec,
        GHGEl ghgEl,
        GHGEp ghgEp,
        GHGEtd ghgEtd,
        GHGEu ghgEu,
        GHGEsca ghgEsca,
        GHGEccs ghgEccs,
        GHGEccr ghgEccr,
        GHGEee ghgEee,
        GHGgCO2eqPerMJ gHGgCO2eqPerMJ,
        FossilFuelComparatorgCO2eqPerMJ fossilFuelComparatorgCO2eqPerMJ,
        GHGEmissionSaving ghgEmissionSaving,
        DeclarationRowNumber declarationRowNumber,
        IncomingDeclarationUploadId incomingDeclarationUploadId)
    {
        return new IncomingDeclarationUpdateParameters(
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
            nUTS2Region,
            totalDefaultValueAccordingToRED2,
            ghgEec,
            ghgEl,
            ghgEp,
            ghgEtd,
            ghgEu,
            ghgEsca,
            ghgEccs,
            ghgEccr,
            ghgEee,
            gHGgCO2eqPerMJ,
            fossilFuelComparatorgCO2eqPerMJ,
            ghgEmissionSaving,
            declarationRowNumber,
            incomingDeclarationUploadId
        );
    }
    public static IncomingDeclarationUpdateParameters Empty()
    {
        return new IncomingDeclarationUpdateParameters();
    }
}