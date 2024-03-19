using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class OutgoingDeclaration : Entity
    {
        public OutgoingDeclarationId OutgoingDeclarationId { get; private set; }
        public IReadOnlyList<IncomingDeclarationIdPairing> IncomingDeclarationPairings { get; private set; }
        public IReadOnlyList<FuelTransactionId> FuelTransactionIds { get; private set; }
        public Country Country { get; private set; }
        public Product Product { get; private set; }
        public CustomerDetails CustomerDetails { get; private set; }
        public VolumeTotal VolumeTotal { get; private set; }
        public AllocationTotal AllocationTotal { get; private set; }
        public GhgReduction GhgReduction { get; private set; }
        public BfeId BfeId { get; private set; }
        public DatePeriod DatePeriod { get; private set; }
        public CertificateId CertificateId { get; private set; }
        public SustainabilityDeclarationNumber SustainabilityDeclarationNumber { get; private set; }
        public DateOfIssuance DateOfIssuance { get; private set; }
        public RawMaterialName RawMaterialName { get; private set; }
        public RawMaterialCode RawMaterialCode { get; private set; }
        public ProductionCountry ProductionCountry { get; private set; }
        public AdditionalInformation AdditionalInformation { get; private set; }
        public Mt Mt { get; private set; }
        public Density Density { get; private set; }
        public Liter Liter { get; private set; }
        public EnergyContent EnergyContent { get; private set; }
        public GreenhouseGasEmission GreenhouseGasEmission { get; private set; }
        public GreenhouseGasReduction GreenhouseGasReduction { get; private set; }
        public EmissionSavingControl EmissionSavingControl { get; private set; }
        public EnergyContentControl EnergyContentControl { get; private set; }
        public FossilFuelComparatorgCO2EqPerMJ FossilFuelComparatorgCO2EqPerMJ { get; private set; }
        public CustomerName CustomerName { get; private set; }
        public DraftAllocationId DraftAllocationId { get; private set; }

        
        public DateOfCreation DateOfCreation { get; private set; }

        private OutgoingDeclaration()
        {
            OutgoingDeclarationId = OutgoingDeclarationId.Empty();
            Id = OutgoingDeclarationId.Value;
            IncomingDeclarationPairings = new List<IncomingDeclarationIdPairing>();
            FuelTransactionIds = new List<FuelTransactionId>();
            Country = Country.Empty();
            Product = Product.Empty();
            CustomerDetails = CustomerDetails.Empty();
            VolumeTotal = VolumeTotal.Empty();
            AllocationTotal = AllocationTotal.Empty();
            GhgReduction = GhgReduction.Empty();
            BfeId = BfeId.Empty();
            Density = Density.Empty();
            DatePeriod = DatePeriod.Empty();
            CertificateId = CertificateId.Empty();
            SustainabilityDeclarationNumber = SustainabilityDeclarationNumber.Empty();
            DateOfIssuance = DateOfIssuance.Empty();
            RawMaterialName = RawMaterialName.Empty();
            RawMaterialCode = RawMaterialCode.Empty();
            ProductionCountry = ProductionCountry.Empty();
            AdditionalInformation = AdditionalInformation.Empty();
            Mt = Mt.Empty();
            Density = Density.Empty();
            Liter = Liter.Empty();
            EnergyContent = EnergyContent.Empty();
            GreenhouseGasEmission = GreenhouseGasEmission.Empty();
            GreenhouseGasReduction = GreenhouseGasReduction.Empty();
            EmissionSavingControl = EmissionSavingControl.Empty();
            EnergyContentControl = EnergyContentControl.Empty();
            FossilFuelComparatorgCO2EqPerMJ = FossilFuelComparatorgCO2EqPerMJ.Empty();
            DateOfCreation = DateOfCreation.Empty();
            CustomerName = CustomerName.Empty();
            DraftAllocationId = DraftAllocationId.Empty();
        }

        private OutgoingDeclaration(
            OutgoingDeclarationId outgoingDeclarationId,
            IReadOnlyList<IncomingDeclarationIdPairing> incomingDeclarationPairings,
            IReadOnlyList<FuelTransactionId> fuelTransactionIds,
            Country country,
            Product product,
            CustomerDetails customerDetails,
            VolumeTotal volumeTotal,
            AllocationTotal allocationTotal,
            GhgReduction ghgReduction,
            BfeId bfeId,
            Density density,
            DatePeriod datePeriod,
            CertificateId certificateId,
            SustainabilityDeclarationNumber sustainabilityDeclarationNumber,
            DateOfIssuance dateOfIssuance,
            RawMaterialName rawMaterialName,
            RawMaterialCode rawMaterialCode,
            ProductionCountry productionCountry,
            AdditionalInformation additionalInformation,
            Mt mt,
            Liter liter,
            EnergyContent energyContent,
            GreenhouseGasEmission greenhouseGasEmission,
            GreenhouseGasReduction greenhouseGasReduction,
            EmissionSavingControl emissionSavingControl,
            EnergyContentControl energyContentControl,
            FossilFuelComparatorgCO2EqPerMJ fossilFuelComparatorgCO2EqPerMJ,
            DateOfCreation dateOfCreation,
            //CustomerName is here due to OrderingBy (dont remove)
            CustomerName customerName,
            DraftAllocationId draftAllocationId
        )
        {
            OutgoingDeclarationId = outgoingDeclarationId;
            Id = OutgoingDeclarationId.Value;
            IncomingDeclarationPairings = incomingDeclarationPairings;
            FuelTransactionIds = fuelTransactionIds;
            Country = country;
            Product = product;
            CustomerDetails = customerDetails;
            VolumeTotal = volumeTotal;
            AllocationTotal = allocationTotal;
            GhgReduction = ghgReduction;
            BfeId = bfeId;
            Density = density;
            DatePeriod = datePeriod;
            CertificateId = certificateId;
            SustainabilityDeclarationNumber = sustainabilityDeclarationNumber;
            DateOfIssuance = dateOfIssuance;
            RawMaterialName = rawMaterialName;
            RawMaterialCode = rawMaterialCode;
            ProductionCountry = productionCountry;
            AdditionalInformation = additionalInformation;
            Mt = mt;
            Liter = liter;
            EnergyContent = energyContent;
            GreenhouseGasEmission = greenhouseGasEmission;
            GreenhouseGasReduction = greenhouseGasReduction;
            EmissionSavingControl = emissionSavingControl;
            EnergyContentControl = energyContentControl;
            FossilFuelComparatorgCO2EqPerMJ = fossilFuelComparatorgCO2EqPerMJ;
            DateOfCreation = dateOfCreation;
            CustomerName = customerName;
            DraftAllocationId = draftAllocationId;
        }

        public static OutgoingDeclaration Create(
            OutgoingDeclarationId outgoingDeclarationId,
            IReadOnlyList<IncomingDeclarationIdPairing> incomingDeclarationPairings,
            IReadOnlyList<FuelTransactionId> fuelTransactionIds,
            Country country,
            Product product,
            CustomerDetails customerDetails,
            VolumeTotal volumeTotal,
            AllocationTotal allocationTotal,
            GhgReduction ghgReduction,
            BfeId bfeId,
            Density density,
            DatePeriod datePeriod,
            CertificateId certificateId,
            SustainabilityDeclarationNumber sustainabilityDeclarationNumber,
            DateOfIssuance dateOfIssuance,
            RawMaterialName rawMaterialName,
            RawMaterialCode rawMaterialCode,
            ProductionCountry productionCountry,
            AdditionalInformation additionalInformation,
            Mt mt,
            Liter liter,
            EnergyContent energyContent,
            GreenhouseGasEmission greenhouseGasEmission,
            GreenhouseGasReduction greenhouseGasReduction,
            EmissionSavingControl emissionSavingControl,
            EnergyContentControl energyContentControl,
            FossilFuelComparatorgCO2EqPerMJ fossilFuelComparatorgCO2EqPerMJ,
            DateOfCreation dateOfCreation,
            CustomerName customerName,
            DraftAllocationId draftAllocationId
        )
        {
            return new OutgoingDeclaration(
                outgoingDeclarationId,
                incomingDeclarationPairings,
                fuelTransactionIds,
                country,
                product,
                customerDetails,
                volumeTotal,
                allocationTotal,
                ghgReduction,
                bfeId,
                density,
                datePeriod,
                certificateId,
                sustainabilityDeclarationNumber,
                dateOfIssuance,
                rawMaterialName,
                rawMaterialCode,
                productionCountry,
                additionalInformation,
                mt,
                liter,
                energyContent,
                greenhouseGasEmission,
                greenhouseGasReduction,
                emissionSavingControl,
                energyContentControl,
                fossilFuelComparatorgCO2EqPerMJ,
                dateOfCreation,
                customerName,
                draftAllocationId
            );
        }

        public static OutgoingDeclaration Empty()
        {
            return new OutgoingDeclaration();
        }
    }
}