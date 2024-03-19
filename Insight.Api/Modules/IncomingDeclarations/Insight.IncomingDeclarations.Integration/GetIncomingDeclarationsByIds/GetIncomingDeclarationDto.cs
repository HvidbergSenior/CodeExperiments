namespace Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds
{
    public class GetIncomingDeclarationsDto
    {
        public List<GetIncomingDeclarationDto> IncomingDeclarations { get; private set; }

        public GetIncomingDeclarationsDto(List<GetIncomingDeclarationDto> incomingDeclarations)
        {
            IncomingDeclarations = incomingDeclarations;
        }
    }

    public sealed class GetIncomingDeclarationDto
    {
        public Guid IncomingDeclarationId { get;  set; } 
        public string Company { get; private set; }
        public string Country { get; private set; }
        public string Product { get; private set; }
        public string Supplier { get; private set; }
        public string RawMaterial { get; private set; }
        public string PosNumber { get; private set; }
        public string CountryOfOrigin { get; private set; }
        public string PlaceOfDispatch { get; private set; }
        public DateOnly DateOfDispatch { get; private set; }
        public DateOnly DateOfIssuance { get; private set; }
        public string DateOfInstallation { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal GhgEmissionSaving { get; private set; }
        public decimal GhGgCO2EqPerMJ { get; private set; }
        public decimal FossilFuelComparatorgCO2EqPerMJ { get; private set; }
        //BatchId is only used when creating an Outgoing Declaration
        public long BatchId { get; set; }
        public string ProductionCountry { get; private set; }
        public string CertificationNumber { get; private set; }
        public string CertificationSystem { get; private set; }
        public bool ComplianceWithSustainabilityCriteria { get; private set; }
        public bool ComplianceWithEuRedMaterialCriteria { get; private set; }
        public bool ComplianceWithIsccMaterialCriteria { get; private set; }
        public string ChainOfCustodyOption { get; private set; }
        public bool TotalDefaultValueAccordingToREDII { get; private set; }
        public string TypeOfProduct { get; private set; }
        public decimal EnergyContent { get; private set; }
        public bool CultivatedAsIntermediateCrop { get; private set; }
        public bool FulfillsMeasuresForLowILUCRiskFeedstocks { get; private set; }
        public bool MeetsDefinitionOfWasteOrResidue { get; private set; }
        public string SpecifyNUTS2Region { get; private set; }
        public decimal CarbonCaptureAndGeologicalStorage{ get; private set; }
        public decimal CarbonCaptureAndReplacement { get; private set; }
        public decimal ExtractionOrCultivation { get; private set; }
        public decimal FuelInUse { get; private set; }
        public decimal LandUse  { get; private set; }
        public decimal Processing { get; private set; }
        public decimal SoilCarbonAccumulation { get; private set; }
        public decimal TotalGHGEmissionFromSupplyAndUseOfFuel { get; private set; }
        public decimal TransportAndDistribution { get; private set; }
        public GetIncomingDeclarationDto(
            Guid incomingDeclarationId,
            string company,
            string country,
            string product,
            string supplier,
            string rawMaterial,
            string posNumber,
            string countryOfOrigin,
            string placeOfDispatch,
            DateOnly dateOfDispatch,
            decimal quantity,
            decimal ghgEmissionSaving,
            long batchId,
            DateOnly dateOfIssuance,
            string dateOfInstallation,
            string productionCountry,
            string certificationNumber,
            string certificationSystem,
            bool complianceWithSustainabilityCriteria,
            bool complianceWithEuRedMaterialCriteria,
            bool complianceWithIsccMaterialCriteria,
            string chainOfCustodyOption,
            bool totalDefaultValueAccordingToRedii,
            string typeOfProduct,
            decimal energyContent,
            bool cultivatedAsIntermediateCrop,
            bool fulfillsMeasuresForLowIlucRiskFeedstocks,
            bool meetsDefinitionOfWasteOrResidue,
            string specifyNuts2Region,
            decimal carbonCaptureAndGeologicalStorage,
            decimal carbonCaptureAndReplacement,
            decimal extractionOrCultivation,
            decimal fuelInUse,
            decimal landUse,
            decimal processing,
            decimal soilCarbonAccumulation,
            decimal totalGHGEmissionFromSupplyAndUseOfFuel,
            decimal transportAndDistribution,
            decimal ghGgCO2EqPerMJ, 
            decimal fossilFuelComparatorgCo2EqPerMj)
        {
            IncomingDeclarationId = incomingDeclarationId;
            Company = company;
            Country = country;
            Product = product;
            Supplier = supplier;
            RawMaterial = rawMaterial;
            PosNumber = posNumber;
            CountryOfOrigin = countryOfOrigin;
            PlaceOfDispatch = placeOfDispatch;
            DateOfDispatch = dateOfDispatch;
            Quantity = quantity;
            GhgEmissionSaving = ghgEmissionSaving;
            BatchId = batchId;
            DateOfIssuance = dateOfIssuance;
            DateOfInstallation = dateOfInstallation;
            ProductionCountry = productionCountry;
            CertificationNumber = certificationNumber;
            CertificationSystem = certificationSystem;
            ComplianceWithEuRedMaterialCriteria = complianceWithEuRedMaterialCriteria;
            ComplianceWithIsccMaterialCriteria = complianceWithIsccMaterialCriteria;
            ComplianceWithSustainabilityCriteria = complianceWithSustainabilityCriteria;
            ChainOfCustodyOption = chainOfCustodyOption;
            TotalDefaultValueAccordingToREDII = totalDefaultValueAccordingToRedii;
            TypeOfProduct = typeOfProduct;
            EnergyContent = energyContent;
            CultivatedAsIntermediateCrop = cultivatedAsIntermediateCrop;
            FulfillsMeasuresForLowILUCRiskFeedstocks = fulfillsMeasuresForLowIlucRiskFeedstocks;
            MeetsDefinitionOfWasteOrResidue = meetsDefinitionOfWasteOrResidue;
            SpecifyNUTS2Region = specifyNuts2Region;
            CarbonCaptureAndGeologicalStorage = carbonCaptureAndGeologicalStorage;
            CarbonCaptureAndReplacement = carbonCaptureAndReplacement;
            ExtractionOrCultivation = extractionOrCultivation;
            FuelInUse = fuelInUse;
            LandUse = landUse;
            Processing = processing;
            SoilCarbonAccumulation = soilCarbonAccumulation;
            TotalGHGEmissionFromSupplyAndUseOfFuel = totalGHGEmissionFromSupplyAndUseOfFuel;
            TransportAndDistribution = transportAndDistribution;
            GhGgCO2EqPerMJ = ghGgCO2EqPerMJ;
            FossilFuelComparatorgCO2EqPerMJ = fossilFuelComparatorgCo2EqPerMj;
        }
    }
}