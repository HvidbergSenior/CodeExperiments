using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Stock;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.OutgoingDeclarations.Domain;
using Insight.Services.AllocationEngine.Domain;
using Insight.UserAccess.Domain.User;
using DatePeriod = Insight.BuildingBlocks.Domain.DatePeriod;
using FuelTransactionId = Insight.FuelTransactions.Domain.FuelTransactionId;
using ProductDescription = Insight.FuelTransactions.Domain.ProductDescription;

namespace Insight.Tests.End2End
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        private static string GetRandomValidProductName()
        {
            var validProductNames = new List<string> { "HVO DIESEL", "B100" };
            return validProductNames.OrderBy(c => Guid.NewGuid()).First();
        }

        public static FuelTransaction FuelTransaction(string customerNumber = "")
        {
            customerNumber = string.IsNullOrEmpty(customerNumber) ? Instance<string>() : customerNumber;

            return FuelTransactions.Domain.FuelTransaction.Create(FuelTransactionId.Create(Instance<Guid>()),
                FuelTransactionPosSystem.Tokheim,
                StationNumber.Create(Instance<string>()),
                StationName.Create(Instance<string>()),
                FuelTransactionDate.Create("2023-12-15"),
                FuelTransactionTime.Create("01:48:48"),
                ProductNumber.Create(Instance<string>()),
                ProductName.Create(GetRandomValidProductName()),
                FuelTransactions.Domain.Quantity.Create(Instance<decimal>()),
                Odometer.Create(Instance<int>()),
                DriverCardNumber.Create(Instance<string>()),
                VehicleCardNumber.Create(Instance<string>()),
                BuildingBlocks.Domain.CustomerNumber.Create(customerNumber),
                BuildingBlocks.Domain.CustomerName.Create(customerNumber),
                SourceETag.Create(Instance<string>()),
                FuelTransactionCountry.Create(Instance<string>()),
                SourceSystemPropertyBag.Create(Instance<string>()),
                SourceSystemId.Create(Instance<Guid>()),
                Location.Create(Instance<string>()),
                CustomerType.Create(Instance<string>()),
                CustomerSegment.Create(Instance<string>()),
                CompanyName.Create(Instance<string>()),
                AccountNumber.Create(Instance<string>()),
                AccountName.Create(Instance<string>()),
                AccountCustomerId.Create(Instance<Guid>()),
                ProductDescription.Create(Instance<string>()),
                ShipToLocation.Create(Instance<string>()),
                Driver.Create(Instance<string>()));
        }

        public static Customers.Domain.Customer Customer()
        {
            return Customers.Domain.Customer.Create(Customers.Domain.CompanyId.Create(Instance<Guid>()),
                BuildingBlocks.Domain.CustomerId.Create(Instance<Guid>()),
                CustomerDetails.Create(BuildingBlocks.Domain.CustomerNumber.Create(Instance<string>()), CustomerAddress.Create(Instance<string>()), CustomerBillToName.Create(Instance<string>()),
                CustomerBillToNumber.Create(Instance<string>()),
                CustomerCity.Create(Instance<string>()),
                CustomerDeliveryType.Create(Instance<string>()),
                CustomerIndustry.Create(Instance<string>()),
                BuildingBlocks.Domain.CustomerName.Create(Instance<string>()),
                CustomerPostCode.Create(Instance<string>()),
                CustomerCountryRegion.Create(Instance<string>())),
                Customers.Domain.BalanceLcy.Create(Instance<decimal>()),
                Customers.Domain.BalanceDueLcy.Create(Instance<decimal>()),
                Customers.Domain.OutstandingOrdersLcy.Create(Instance<decimal>()),
                Customers.Domain.NumberNumber.Create(Instance<string>()),
                Customers.Domain.PdiAndLdPointNumber.Create(Instance<string>()),
                Customers.Domain.VatRegNumber.Create(Instance<string>()),
                Customers.Domain.OrganisationNumber.Create(Instance<string>()),
                Customers.Domain.PaymentTermsCode.Create(Instance<string>()),
                Customers.Domain.ShipmentMethodCode.Create(Instance<string>()),
                Customers.Domain.ShippingAgentCode.Create(Instance<string>()),
                Customers.Domain.SalesPerson.Create(Instance<string>()),
                Customers.Domain.SourceETag.Create(Instance<string>()),
                Customers.Domain.CreditLimit.Create(Instance<decimal>()),
                Customers.Domain.Blocked.Create(Instance<string>()),
                Customers.Domain.CardCustomer.Create(Instance<bool>()));
        }

        internal static StockTransaction StockTransaction(ProductName productName, CompanyName companyName)
        {
            return FuelTransactions.Domain.Stock.StockTransaction.Create(StockTransactionId.Create(Instance<Guid>()),                               
                                              Location.Create(Instance<string>()),
                                              ProductNumber.Create(Instance<string>()),
                                              productName,
                                              FuelTransactions.Domain.Quantity.Create(Instance<decimal>()),
                                              StockTransactionDate.Create(DateOnly.FromDateTime(DateTime.Now)),
                                              StockCompanyId.Create(Instance<Guid>()),
                                              companyName);
        }

        internal static FuelTransactionsBetweenDates FuelTransactionsBetweenDates()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;

            return FuelTransactions.Domain.FuelTransactionsBetweenDates.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
        }
        internal static IncomingDeclarations.Domain.Incoming.FilteringParameters IncomingFilteringParameters()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;
            var datePeriod = IncomingDeclarations.Domain.Incoming.DatePeriod.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
            var product = IncomingDeclarations.Domain.Incoming.Product.Create(Instance<string>());
            var supplier = IncomingDeclarations.Domain.Incoming.Supplier.Create(Instance<string>());
            var company = IncomingDeclarations.Domain.Incoming.Company.Create(Instance<string>());

            return IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(datePeriod, product, company, supplier);
        }
        internal static AutoAllocationFilteringParameters AutoAllocationFilteringParameters(FuelTransaction first)
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;
            var product = FilterProductName.Create(first.ProductName.Value);
            var customer = FilterCustomerName.Create(first.CustomerName.Value);
            var company = FilterCompanyName.Create(first.CompanyName.Value);

            return Insight.Services.AllocationEngine.Domain.AutoAllocationFilteringParameters.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now), product, company, customer);
        }
        internal static OutgoingDeclarations.Domain.FilteringParameters OutgoingFilteringParameters()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;
            var datePeriod = BuildingBlocks.Domain.DatePeriod.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
            var product = OutgoingDeclarations.Domain.Product.Create(Instance<string>());
            var company = OutgoingDeclarations.Domain.Company.Create(Instance<string>());
            var customerName = BuildingBlocks.Domain.CustomerName.Create(Instance<string>());

            return OutgoingDeclarations.Domain.FilteringParameters.Create(datePeriod, product, company, customerName);
        }
        internal static OutgoingDeclarations.Domain.FilteringParameters OutgoingFilteringParameters(DatePeriod datePeriod, OutgoingDeclarations.Domain.Product product, CustomerName customerName, OutgoingDeclarations.Domain.Company company)
        {
            return OutgoingDeclarations.Domain.FilteringParameters.Create(datePeriod, product, company, customerName);
        }
      
        internal static FuelTransactions.Domain.FilteringParameters OutgoingFuelTransactionsParameters()
        {
            any.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-10), maxDate: DateTime.Now.AddYears(-5)));
            var fromDate = any.Create<DateTime>();
            var now = DateTime.Now;
            var datePeriod = BuildingBlocks.Domain.DatePeriod.Create(DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(now));
            var productName = ProductName.Create(Instance<string>());
            var customerName = BuildingBlocks.Domain.CustomerName.Create(Instance<string>());
            var companyName = CompanyName.Create(Instance<string>());
            return FuelTransactions.Domain.FilteringParameters.Create(datePeriod, productName, customerName, companyName);
        }
      
        internal static IncomingDeclarationUpdateParameters IncomingDeclarationUpdateParameters()
        {
            var company = IncomingDeclarationCompany();
            var country = IncomingDeclarationCountry();
            var product = ProductIncoming();
            var dateOfDispatch = DateOfDispatch();
            var supplier = IncomingDeclarationSupplier();
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
            var nuts2Region = SpecifyNUTS2Region();
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
                incomingDeclarationUploadId
                );
        }

        internal static UserUpdateParameters UserUpdateParameters(Guid userId)
        {
            return UserAccess.Domain.User.UserUpdateParameters.Create(
                UserName.Create(Instance<string>()),
                UserId.Create(userId),
                FirstName.Create(Instance<string>()),
                LastName.Create(Instance<string>()),
                Email.Create($"{Instance<string>()}@{Instance<string>()}"),
                UserRole.User,
                UserStatus.Active
            );
        }
        internal static IncomingDeclaration IncomingDeclaration()
        {
            var incomingDeclarationId = IncomingDeclarationId();
            var company = IncomingDeclarationCompany();
            var country = IncomingDeclarationCountry();
            var product = ProductIncoming();
            var dateOfDispatch = DateOfDispatch();
            var supplier = IncomingDeclarationSupplier();
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
            var chainOfCustodyOption = ChainOfCustodyOption();
            var cultivatedAsIntermediateCrop = CultivatedAsIntermediateCrop();
            var fulfillsMeasuresForLowIlucRiskFeedstocks = FulfillsMeasuresForLowILUCRiskFeedstocks();
            var meetsDefinitionOfWasteOrResidue = MeetsDefinitionOfWasteOrResidue();
            var nuts2Region = SpecifyNUTS2Region();
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
            const IncomingDeclarationState incomingDeclarationState = IncomingDeclarationState.New;
            var sourceFormatPropertyBag = SourceFormatPropertyBag();

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
         internal static OutgoingDeclaration OutgoingDeclaration()
         {
             var outgoingDeclarationId = OutgoingDeclarationId();
             var incomingDeclarationPairings = IncomingDeclarationPairings();
             var fuelTransactionIds = FuelTransactionIds();
             var country = CountryOutgoing();
             var product = ProductOutgoing();
             var customerNumber = CustomerNumber();
             var customerName = CustomerName();
             var allocationTotal = AllocationTotal();
             var volumeTotal = VolumeTotal(); 
             var ghgReduction = GhgReduction();
             var bfeId = BfeId();
             var certificateId = CertificateId();
             var sustainabilityDeclarationNumber = SustainabilityDeclarationNumber();
             var dateOfIssuance = DateOfIssuanceOutgoing();
             var rawMaterialName = RawMaterialName();
             var rawMaterialCode = RawMaterialCode();
             var productionCountry = ProductionCountryOutgoing();
             var additionalInformation = AdditionalInformationOutgoing();
             var mt = Mt();
             var density = Density();
             var liter = Liter();
             var energyContent = EnergyContent();
             var greenhouseGasEmission = GreenhouseGasEmission();
             var greenhouseGasReduction = GreenhouseGasReduction();
             var emissionSavingControl = EmissionSavingControl();
             var energyContentControl = EnergyContentControl();
             var customerDetails = Customer().CustomerDetails;
             var fossilFuelComparatorgCO2EqPerMJ = FossilFuelComparatorgCO2EqPerMJ();
             var dateOfCreation = DateOfCreation();
             var draftAllocationId = DraftAllocationId();
             var datePeriod = DatePeriod();
             return OutgoingDeclarations.Domain.OutgoingDeclaration.Create(
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

        private static FossilFuelComparatorgCO2EqPerMJ FossilFuelComparatorgCO2EqPerMJ()
        {
            return OutgoingDeclarations.Domain.FossilFuelComparatorgCO2EqPerMJ.Create(Instance<decimal>());
        }

        private static CustomerNumber CustomerNumber()
        {
            return BuildingBlocks.Domain.CustomerNumber.Create(Instance<string>());
        }
        private static SourceFormatPropertyBag SourceFormatPropertyBag()
        {
            return IncomingDeclarations.Domain.Incoming.SourceFormatPropertyBag.Create(Instance<string>());
        }
        private static CustomerName CustomerName()
        {
            return BuildingBlocks.Domain.CustomerName.Create(Instance<string>());
        }
        private static OutgoingDeclarationId OutgoingDeclarationId()
        {
            return OutgoingDeclarations.Domain.OutgoingDeclarationId.Create(Instance<Guid>());
        }
        private static List<IncomingDeclarationIdPairing> IncomingDeclarationPairings()
        {
            return new List<IncomingDeclarationIdPairing> { OutgoingDeclarations.Domain.IncomingDeclarationIdPairing.Create(OutgoingDeclarations.Domain.IncomingDeclarationId.Create(Instance<Guid>()), OutgoingDeclarations.Domain.Quantity.Create(Instance<decimal>()), OutgoingDeclarations.Domain.BatchId.Create(Instance<long>()))};
        }
        private static List<OutgoingDeclarations.Domain.FuelTransactionId> FuelTransactionIds()
        {
            return new List<OutgoingDeclarations.Domain.FuelTransactionId> { OutgoingDeclarations.Domain.FuelTransactionId.Create(Instance<Guid>()) };
        }
        private static IncomingDeclarations.Domain.Incoming.IncomingDeclarationId IncomingDeclarationId()
        {
            return IncomingDeclarations.Domain.Incoming.IncomingDeclarationId.Create(Instance<Guid>());
        }
        private static IncomingDeclarations.Domain.Incoming.Company IncomingDeclarationCompany()
        {
            return IncomingDeclarations.Domain.Incoming.Company.Create(Instance<string>());
        }
        private static IncomingDeclarations.Domain.Incoming.Country IncomingDeclarationCountry()
        {
            return IncomingDeclarations.Domain.Incoming.Country.Create(Instance<string>());
        }
        private static OutgoingDeclarations.Domain.Country CountryOutgoing()
        {
            return OutgoingDeclarations.Domain.Country.Create(Instance<string>());
        }
        private static OutgoingDeclarations.Domain.Product ProductOutgoing()
        {
            return OutgoingDeclarations.Domain.Product.Create(Instance<string>());
        }
        private static AllocationTotal AllocationTotal()
        {
            return OutgoingDeclarations.Domain.AllocationTotal.Create(Instance<decimal>());
        }  
        private static VolumeTotal VolumeTotal()
        {
            return OutgoingDeclarations.Domain.VolumeTotal.Create(Instance<decimal>());
        }
        private static OutgoingDeclarations.Domain.CertificationSystem CertificationSystemOutgoing()
        {
            return OutgoingDeclarations.Domain.CertificationSystem.Create(Instance<string>());
        }
        private static GhgReduction GhgReduction()
        {
            return OutgoingDeclarations.Domain.GhgReduction.Create(Instance<decimal>());
        }
        private static OutgoingDeclarations.Domain.RawMaterial RawMaterialOutgoing()
        {
            return OutgoingDeclarations.Domain.RawMaterial.Create(Instance<string>());
        }
        private static OutgoingDeclarations.Domain.CountryOfOrigin CountryOfOriginOutgoing()
        {
            return OutgoingDeclarations.Domain.CountryOfOrigin.Create(Instance<string>());
        }
        private static Storage Storage()
        {
            return OutgoingDeclarations.Domain.Storage.Create(Instance<string>());
        }  
        private static BfeId BfeId()
        {
            return OutgoingDeclarations.Domain.BfeId.Create(Instance<string>());
        } 
        private static DatePeriod DatePeriod()
        {
            return BuildingBlocks.Domain.DatePeriod.Create(DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue);
        } 
        private static OutgoingDeclarations.Domain.Supplier SupplierOutgoing()
        {
            return OutgoingDeclarations.Domain.Supplier.Create(Instance<string>());
        } 
        private static CertificateId CertificateId()
        {
            return OutgoingDeclarations.Domain.CertificateId.Create(Instance<string>());
        } 
        private static SustainabilityDeclarationNumber SustainabilityDeclarationNumber()
        {
            return OutgoingDeclarations.Domain.SustainabilityDeclarationNumber.Create(Instance<string>());
        } 
        private static OutgoingDeclarations.Domain.DateOfIssuance DateOfIssuanceOutgoing()
        {
            return OutgoingDeclarations.Domain.DateOfIssuance.Create(DateOnly.FromDateTime(DateTime.Now));
        } 
        private static DateOfCreation DateOfCreation()
        {
            return OutgoingDeclarations.Domain.DateOfCreation.Create(DateOnly.FromDateTime(DateTime.Now));
        } 
        private static DraftAllocationId DraftAllocationId()
        {
            return FuelTransactions.Domain.DraftAllocationId.Create(Instance<Guid>());
        } 
        private static RawMaterialName RawMaterialName()
        {
            return OutgoingDeclarations.Domain.RawMaterialName.Create(Instance<string>());
        } 
        private static RawMaterialCode RawMaterialCode()
        {
            return OutgoingDeclarations.Domain.RawMaterialCode.Create(Instance<string>());
        } 
       
        private static Mt Mt()
        {
            return OutgoingDeclarations.Domain.Mt.Create(Instance<decimal>());
        } 
        private static Density Density()
        {
            return OutgoingDeclarations.Domain.Density.Create(Instance<decimal>());
        } 
        private static Liter Liter()
        {
            return OutgoingDeclarations.Domain.Liter.Create(Instance<decimal>());
        } 
        private static EnergyContent EnergyContent()
        {
            return OutgoingDeclarations.Domain.EnergyContent.Create(Instance<decimal>());
        } 
        private static GreenhouseGasEmission GreenhouseGasEmission()
        {
            return OutgoingDeclarations.Domain.GreenhouseGasEmission.Create(Instance<decimal>());
        } 
        private static GreenhouseGasReduction GreenhouseGasReduction()
        {
            return OutgoingDeclarations.Domain.GreenhouseGasReduction.Create(Instance<decimal>());
        } 
        private static EmissionSavingControl EmissionSavingControl()
        {
            return OutgoingDeclarations.Domain.EmissionSavingControl.Create(Instance<decimal>());
        } 
        private static EnergyContentControl EnergyContentControl()
        {
            return OutgoingDeclarations.Domain.EnergyContentControl.Create(Instance<decimal>());
        } 
        private static OutgoingDeclarations.Domain.PlaceOfDispatch PlaceOfDispatchOutgoing()
        {
            return OutgoingDeclarations.Domain.PlaceOfDispatch.Create(Instance<string>());
        }
        private static OutgoingDeclarations.Domain.AdditionalInformation AdditionalInformationOutgoing()
        {
            return OutgoingDeclarations.Domain.AdditionalInformation.Create(Instance<string>());
        }
        private static OutgoingDeclarations.Domain.ProductionCountry ProductionCountryOutgoing()
        {
            return OutgoingDeclarations.Domain.ProductionCountry.Create(Instance<string>());
        }
        private static IncomingDeclarations.Domain.Incoming.Product ProductIncoming()
        {
            return IncomingDeclarations.Domain.Incoming.Product.Create(Instance<string>());

        }

        private static DateOfDispatch DateOfDispatch()
        {
            return IncomingDeclarations.Domain.Incoming.DateOfDispatch.Create(DateOnly.MaxValue);
        }

        private static IncomingDeclarations.Domain.Incoming.Supplier IncomingDeclarationSupplier()
        {
            return IncomingDeclarations.Domain.Incoming.Supplier.Create(Instance<string>());

        }

        private static IncomingDeclarations.Domain.Incoming.CertificationSystem CertificationSystem()
        {
            return IncomingDeclarations.Domain.Incoming.CertificationSystem.Create(Instance<string>());

        }

        private static SupplierCertificateNumber SupplierCertificateNumber()
        {
            return IncomingDeclarations.Domain.Incoming.SupplierCertificateNumber.Create(Instance<string>());
        }

        private static PosNumber PosNumber()
        {
            return IncomingDeclarations.Domain.Incoming.PosNumber.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.DateOfIssuance DateOfIssuance()
        {
            return IncomingDeclarations.Domain.Incoming.DateOfIssuance.Create(DateOnly.MaxValue);
        }

        private static IncomingDeclarations.Domain.Incoming.PlaceOfDispatch PlaceOfDispatch()
        {
            return IncomingDeclarations.Domain.Incoming.PlaceOfDispatch.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.ProductionCountry ProductionCountry()
        {
            return IncomingDeclarations.Domain.Incoming.ProductionCountry.Create(Instance<string>());
        }

        private static DateOfInstallation DateOfInstallation()
        {
            return IncomingDeclarations.Domain.Incoming.DateOfInstallation.Create(Instance<string>());
        }

        private static TypeOfProduct TypeOfProduct()
        {
            return IncomingDeclarations.Domain.Incoming.TypeOfProduct.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.RawMaterial RawMaterial()
        {
            return IncomingDeclarations.Domain.Incoming.RawMaterial.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.AdditionalInformation AdditionalInformation()
        {
            return IncomingDeclarations.Domain.Incoming.AdditionalInformation.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.CountryOfOrigin CountryOfOrigin()
        {
            return IncomingDeclarations.Domain.Incoming.CountryOfOrigin.Create(Instance<string>());
        }

        private static IncomingDeclarations.Domain.Incoming.Quantity Quantity()
        {
            return IncomingDeclarations.Domain.Incoming.Quantity.Create(Instance<decimal>());
        }

        private static UnitOfMeasurement UnitOfMeasurement()
        {
            return IncomingDeclarations.Domain.Incoming.UnitOfMeasurement.Litres;
        }

        private static EnergyContentMJ EnergyContentMJ()
        {
            return IncomingDeclarations.Domain.Incoming.EnergyContentMJ.Create(Instance<decimal>());
        }

        private static EnergyQuantityGJ EnergyQuantityGJ()
        {
            return IncomingDeclarations.Domain.Incoming.EnergyQuantityGJ.Create(Instance<decimal>());
        }

        private static ComplianceWithSustainabilityCriteria ComplianceWithSustainabilityCriteria()
        {
            return IncomingDeclarations.Domain.Incoming.ComplianceWithSustainabilityCriteria.Create(Instance<bool>());
        }
        private static ComplianceWithEuRedMaterialCriteria ComplianceWithEuRedMaterialCriteria()
        {
            return IncomingDeclarations.Domain.Incoming.ComplianceWithEuRedMaterialCriteria.Create(Instance<bool>());
        }  
        private static ComplianceWithIsccMaterialCriteria ComplianceWithIsccMaterialCriteria()
        {
            return IncomingDeclarations.Domain.Incoming.ComplianceWithIsccMaterialCriteria.Create(Instance<bool>());
        }  
        private static ChainOfCustodyOption ChainOfCustodyOption()
        {
            return IncomingDeclarations.Domain.Incoming.ChainOfCustodyOption.Create(Instance<string>());
        }

        private static CultivatedAsIntermediateCrop CultivatedAsIntermediateCrop()
        {
            return IncomingDeclarations.Domain.Incoming.CultivatedAsIntermediateCrop.Create(Instance<bool>());
        }

        private static FulfillsMeasuresForLowILUCRiskFeedstocks FulfillsMeasuresForLowILUCRiskFeedstocks()
        {
            return IncomingDeclarations.Domain.Incoming.FulfillsMeasuresForLowILUCRiskFeedstocks.Create(Instance<bool>());
        }

        private static MeetsDefinitionOfWasteOrResidue MeetsDefinitionOfWasteOrResidue()
        {
            return IncomingDeclarations.Domain.Incoming.MeetsDefinitionOfWasteOrResidue.Create(Instance<bool>());
        }

        private static NUTS2Region SpecifyNUTS2Region()
        {
            return NUTS2Region.Create(Instance<string>());
        }

        private static TotalDefaultValueAccordingToRED2 TotalDefaultValueAccordingToREDII()
        {
            return TotalDefaultValueAccordingToRED2.Create(Instance<bool>());
        }

        private static GHGEec GHGEec()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEec.Create(Instance<decimal>());
        }

        private static GHGEl GHGEl()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEl.Create(Instance<decimal>());
        }

        private static GHGEp GHGEp()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEp.Create(Instance<decimal>());
        }

        private static GHGEtd GHGEtd()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEtd.Create(Instance<decimal>());
        }

        private static GHGEu GHGEu()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEu.Create(Instance<decimal>());
        }

        private static GHGEsca GHGEsca()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEsca.Create(Instance<decimal>());
        }

        private static GHGEccs GHGEccs()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEccs.Create(Instance<decimal>());
        }

        private static GHGEccr GHGEccr()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEccr.Create(Instance<decimal>());
        }

        private static GHGEee GHGEee()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEee.Create(Instance<decimal>());
        }

        private static GHGgCO2eqPerMJ GHGgCO2eqPerMJ()
        {
            return IncomingDeclarations.Domain.Incoming.GHGgCO2eqPerMJ.Create(Instance<decimal>());
        }

        private static FossilFuelComparatorgCO2eqPerMJ FossilFuelComparatorgCO2eqPerMJ()
        {
            return IncomingDeclarations.Domain.Incoming.FossilFuelComparatorgCO2eqPerMJ.Create(Instance<decimal>());
        }

        private static GHGEmissionSaving GHGEmissionSaving()
        {
            return IncomingDeclarations.Domain.Incoming.GHGEmissionSaving.Create(Instance<decimal>());
        }

        private static DeclarationRowNumber RowNumber()
        {
            return DeclarationRowNumber.Create(Instance<int>());
        }

        private static IncomingDeclarationUploadId IncomingDeclarationUploadId()
        {
            return IncomingDeclarations.Domain.Incoming.IncomingDeclarationUploadId.Create(Instance<Guid>());
        }
    }
}
