using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.OutgoingDeclarations.Domain;
using FilteringParameters = Insight.OutgoingDeclarations.Domain.FilteringParameters;
using FuelTransactionId = Insight.OutgoingDeclarations.Domain.FuelTransactionId;
using Quantity = Insight.OutgoingDeclarations.Domain.Quantity;

namespace Insight.OutgoingDeclarations.Test
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        internal static FilteringParameters FilteringParameters()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10),
                maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;
            var datePeriod = BuildingBlocks.Domain.DatePeriod.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
            var product = Domain.Product.Create(Instance<string>());
            var company = Domain.Company.Create(Instance<string>());
            var customerName = BuildingBlocks.Domain.CustomerName.Create(Instance<string>());

            return Domain.FilteringParameters.Create(datePeriod, product, company, customerName);
        }

        internal static OutgoingDeclaration OutgoingDeclaration()
        {
            var outgoingDeclarationId = OutgoingDeclarationId();
            var incomingDeclarationPairings = IncomingDeclarationIdPairings();
            var fuelTransactionIds = FuelTransactionIds();
            var country = Country();
            var product = Product();
            var customerDetails = CustomerDetails();
            var volumeTotal = VolumeTotal();
            var allocationTotal = AllocationTotal();
            var ghgReduction = GhgReduction();
            var bfeId = BfeId();
            var certificateId = CertificateId();
            var sustainabilityDeclarationNumber = SustainabilityDeclarationNumber();
            var dateOfIssuance = DateOfIssuance();
            var rawMaterialName = RawMaterialName();
            var rawMaterialCode = RawMaterialCode();
            var productionCountry = ProductionCountry();
            var additionalInformation = AdditionalInformation();
            var mt = Mt();
            var density = Density();
            var liter = Liter();
            var energyContent = EnergyContent();
            var greenhouseGasEmission = GreenhouseGasEmission();
            var greenhouseGasReduction = GreenhouseGasReduction();
            var emissionSavingControl = EmissionSavingControl();
            var energyContentControl = EnergyContentControl();
            var fossilFuelComparatorgCO2EqPerMJ = FossilFuelComparatorgCO2EqPerMJ();
            var dateOfCreation = DateOfCreation();
            var draftAllocationId = DraftAllocationId();
            var customerName = CustomerName();
            var datePeriod = DatePeriod();
            return Domain.OutgoingDeclaration.Create(
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
        internal static OutgoingDeclaration OutgoingDeclaration(DateOfIssuance dateOfIssuance, Product product, CustomerName customerName)
        {
            var outgoingDeclarationId = OutgoingDeclarationId();
            var incomingDeclarationIdsPairings = IncomingDeclarationIdPairings();
            var fuelTransactionIds = FuelTransactionIds();
            var country = Country();
            var volumeTotal = VolumeTotal();
            var allocationTotal = AllocationTotal();
            var ghgReduction = GhgReduction();
            var bfeId = BfeId();
            var certificateId = CertificateId();
            var sustainabilityDeclarationNumber = SustainabilityDeclarationNumber();
            var rawMaterialName = RawMaterialName();
            var rawMaterialCode = RawMaterialCode();
            var productionCountry = ProductionCountry();
            var additionalInformation = AdditionalInformation();
            var mt = Mt();
            var density = Density();
            var liter = Liter();
            var energyContent = EnergyContent();
            var greenhouseGasEmission = GreenhouseGasEmission();
            var greenhouseGasReduction = GreenhouseGasReduction();
            var emissionSavingControl = EmissionSavingControl();
            var energyContentControl = EnergyContentControl();
            var fossilFuelComparatorgCO2EqPerMJ = FossilFuelComparatorgCO2EqPerMJ();
            var dateOfCreation = DateOfCreation();
            var customerDetails = CustomerDetails();
            var draftAllocationId = DraftAllocationId();
            var datePeriod = DatePeriod();
            return Domain.OutgoingDeclaration.Create(
                outgoingDeclarationId,
                incomingDeclarationIdsPairings,
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
        internal static FossilFuelComparatorgCO2EqPerMJ FossilFuelComparatorgCO2EqPerMJ()
        {
            return Domain.FossilFuelComparatorgCO2EqPerMJ.Create(Instance<decimal>());
        }
        internal static OutgoingDeclaration OutgoingDeclaration(Product product)
        {
            return OutgoingDeclaration(DateOfIssuance(), product, CustomerName());
        }
        internal static OutgoingDeclaration OutgoingDeclaration(CustomerName customerName)
        {
            return OutgoingDeclaration(DateOfIssuance(), Product(), customerName);
        }
        internal static OutgoingDeclaration OutgoingDeclaration(Company company)
        {
            return OutgoingDeclaration(DateOfIssuance(), Product(), CustomerName());
        }
        internal static OutgoingDeclaration OutgoingDeclaration(DateOfIssuance dateOfIssuance)
        {
            return OutgoingDeclaration(dateOfIssuance, Product(), CustomerName());
        }
        private static OutgoingDeclarationId OutgoingDeclarationId()
        {
            return Domain.OutgoingDeclarationId.Create(Instance<Guid>());
        }
        private static List<IncomingDeclarationIdPairing> IncomingDeclarationIdPairings()
        {
            return new List<IncomingDeclarationIdPairing> { Domain.IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(Instance<Guid>()), Quantity.Create(Instance<decimal>()), Domain.BatchId.Create(Instance<long>()))};
        }
        private static List<FuelTransactionId> FuelTransactionIds()
        {
            return new List<FuelTransactionId> { Domain.FuelTransactionId.Create(Instance<Guid>()) };
        }
        private static AllocationTotal AllocationTotal()
        {
            return Domain.AllocationTotal.Create(Instance<decimal>());
        } 
        private static VolumeTotal VolumeTotal()
        {
            return Domain.VolumeTotal.Create(Instance<decimal>());
        } 
        private static CountryOfOrigin CountryOfOrigin()
        {
            return Domain.CountryOfOrigin.Create(Instance<string>());
        } 
        private static GhgReduction GhgReduction()
        {
            return Domain.GhgReduction.Create(Instance<decimal>());
        } 
        private static RawMaterial RawMaterial()
        {
            return Domain.RawMaterial.Create(Instance<string>());
        } 
        private static CertificationSystem CertificationSystem()
        {
            return Domain.CertificationSystem.Create(Instance<string>());
        } 
        private static Country Country()
        {
            return Domain.Country.Create(Instance<string>());
        } 
        private static Company Company()
        {
            return Domain.Company.Create(Instance<string>());
        }  
        private static CustomerNumber CustomerNumber()
        {
            return BuildingBlocks.Domain.CustomerNumber.Create(Instance<string>());
        }  
        private static CustomerName CustomerName()
        {
            return BuildingBlocks.Domain.CustomerName.Create(Instance<string>());
        }  
        private static Product Product()
        {
            return Domain.Product.Create(Instance<string>());
        }
        private static CustomerDetails CustomerDetails()
        {
            return BuildingBlocks.Domain.CustomerDetails.Create(CustomerNumber(), CustomerAddress.Empty(), CustomerBillToName.Empty(), CustomerBillToNumber.Empty(), CustomerCity.Empty(), CustomerDeliveryType.Empty(), CustomerIndustry.Empty(), CustomerName(), CustomerPostCode.Empty(), CustomerCountryRegion.Empty());
        }
        private static Storage Storage()
        {
            return Domain.Storage.Create(Instance<string>());
        } 
        private static BfeId BfeId()
        {
            return Domain.BfeId.Create(Instance<string>());
        } 
        private static Supplier Supplier()
        {
            return Domain.Supplier.Create(Instance<string>());
        } 
        private static DatePeriod DatePeriod()
        {
            return BuildingBlocks.Domain.DatePeriod.Create(DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue);
        } 

        private static CertificationSystem CertificateSystem()
        {
            return Domain.CertificationSystem.Create(Instance<string>());
        } 

        private static CertificateId CertificateId()
        {
            return Domain.CertificateId.Create(Instance<string>());
        } 

        private static SustainabilityDeclarationNumber SustainabilityDeclarationNumber()
        {
            return Domain.SustainabilityDeclarationNumber.Create(Instance<string>());
        } 

        private static DateOfIssuance DateOfIssuance()
        {
            return Domain.DateOfIssuance.Create(DateOnly.FromDateTime(DateTime.Now));
        } 
        private static DateOfCreation DateOfCreation()
        {
            return Domain.DateOfCreation.Create(DateOnly.FromDateTime(DateTime.Now));
        } 
        private static DraftAllocationId DraftAllocationId()
        {
            return  Insight.FuelTransactions.Domain.DraftAllocationId.Create(Instance<Guid>());
        } 

        private static PlaceOfDispatch PlaceOfDispatch()
        {
            return Domain.PlaceOfDispatch.Create(Instance<string>());
        } 

        private static RawMaterialName RawMaterialName()
        {
            return Domain.RawMaterialName.Create(Instance<string>());
        } 

        private static RawMaterialCode RawMaterialCode()
        {
            return Domain.RawMaterialCode.Create(Instance<string>());
        } 

        private static ProductionCountry ProductionCountry()
        {
            return Domain.ProductionCountry.Create(Instance<string>());
        } 

        private static AdditionalInformation AdditionalInformation()
        {
            return Domain.AdditionalInformation.Create(Instance<string>());
        } 

        private static Mt Mt()
        {
            return Domain.Mt.Create(Instance<decimal>());
        } 

        private static Density Density()
        {
            return Domain.Density.Create(Instance<decimal>());
        } 

        private static Liter Liter()
        {
            return Domain.Liter.Create(Instance<decimal>());
        } 

        private static EnergyContent EnergyContent()
        {
            return Domain.EnergyContent.Create(Instance<decimal>());
        } 

        private static GreenhouseGasEmission GreenhouseGasEmission()
        {
            return Domain.GreenhouseGasEmission.Create(Instance<decimal>());
        } 

        private static GreenhouseGasReduction GreenhouseGasReduction()
        {
            return Domain.GreenhouseGasReduction.Create(Instance<decimal>());
        } 

        private static EmissionSavingControl EmissionSavingControl()
        {
            return Domain.EmissionSavingControl.Create(Instance<decimal>());
        } 

        private static EnergyContentControl EnergyContentControl()
        {
            return Domain.EnergyContentControl.Create(Instance<decimal>());
        } 

       
        
    }
}