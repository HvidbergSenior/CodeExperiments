using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.Customers.Application.AllowedRawMaterials;
using Insight.Customers.Application.CO2Targets;
using Insight.Customers.Domain;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using CustomerId = Insight.BuildingBlocks.Domain.CustomerId;

namespace Insight.Customers.Tests
{
    internal static class Any
    {
        private static readonly Fixture any = new();

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        internal static CO2TargetUpdatedCommand CO2TargetUpdatedCommand()
        {
            return Customers.Application.CO2Targets.CO2TargetUpdatedCommand.Create(Any.CustomerNumber(), Any.CompanyId(), Any.CO2Target());
        }

        internal static AllowedRawMaterialUpdatedCommand AllowedRawMaterialUpdatedCommand()
        {
            return Customers.Application.AllowedRawMaterials.AllowedRawMaterialUpdatedCommand.Create(CompanyId(), CustomerNumber(), AllowedRawMaterials());
        }

        internal static CompanyId CompanyId()
        {
            return Customers.Domain.CompanyId.Create(Instance<Guid>());
        }

        internal static AllowedRawMaterials AllowedRawMaterials()
        {
            return Customers.Domain.AllowedRawMaterials.Create(Instance<Dictionary<string, bool>>());
        }

        internal static CO2Target CO2Target()
        {
            return Domain.CO2Target.Create(Instance<decimal>());
        }

        internal static CustomerNumber CustomerNumber()
        {
            return BuildingBlocks.Domain.CustomerNumber.Create(Instance<string>());
        }        

        internal static BusinessCentralCo2Target BusinessCentralCo2Target()
        {
            return new BusinessCentralCo2Target()
            {
                Co2Target = Instance<decimal>(),                
                CustomerNumber= Instance<string>(),
                Etag = Instance<string>()
            };
        }

        internal static BusinessCentralRawMaterial BusinessCentralRawMaterial()
        {
            return new BusinessCentralRawMaterial()
            {
                CustomerNumber = Instance<string>(),
                Etag = Instance<string>(),
                Feedstock = Instance<string>(),
                IncludeExclude = Instance<string>()                
            };
        }

        internal static BusinessCentralCustomer BusinessCentralCustomer()
        {
            return new BusinessCentralCustomer()
            {
                Blocked = Instance<string>(),
                CustomerAddress= Instance<string>(),
                CustomerCity= Instance<string>(),
                CustomerBillToName= Instance<string>(),
                CustomerBillToNumber= Instance<string>(),
                CustomerCountryRegion= Instance<string>(),
                CustomerDeliveryType= Instance<string>(),
                CustomerIndustry= Instance<string>(),   
                CustomerName= Instance<string>(),
                CustomerNumber= Instance<string>(),
                CustomerPostCode= Instance<string>(),
                Etag= Instance<string>(),
                Number= Instance<string>(),
                OrganisationNumber= Instance<string>(),
                PaymentTermsCode = Instance<string>(),
                PDIAndLDPointNumber = Instance<string>(),
                SalesPerson = Instance<string>(),
                ShipmentMethodCode= Instance<string>(),
                ShippingAgentCode = Instance<string>(),
                VATRegNumber = Instance<string>(),
                BalanceDueLCY = Instance<decimal>(),
                BalanceLCY = Instance<decimal>(),
                CardCustomer = Instance<bool>(),                
                CreditLimit = Instance<decimal>(),
                OutstandingOrdersLCY = Instance<decimal>()
            };
        }
        
        internal static Customer Customer(Guid? specificCustomerId = null)
        {
            var companyId = CompanyId();
            var customerId = specificCustomerId == null ? CustomerId() : BuildingBlocks.Domain.CustomerId.Create((Guid)specificCustomerId);
            var customerDetails = CustomerDetails();
            var salesPerson = SalesPerson();
            var sourceEtag = SourceEtag();
            var creditLimit = CreditLimit();
            var blocked = Blocked();
            var balanceLcy = BalanceLcy();
            var balanceDueLcy = BalanceDueLcy();
            var outstandingOrdersLcy = OutstandingOrdersLcy();
            var numberNumber = NumberNumber();
            var organisationNumber = OrganisationNumber();
            var pdiAndLdPointNumber = PdiAndLdPointNumber();
            var vatRegNumber = VatRegNumber();
            var paymentTermsCode = PaymentTermsCode();
            var shipmentMethodCode = ShipmentMethodCode();
            var shippingAgentCode = ShippingAgentCode();
            var cardCustomer = CardCustomer();
            return Customers.Domain.Customer.Create(companyId, customerId, customerDetails, balanceLcy, balanceDueLcy,
                outstandingOrdersLcy, numberNumber, pdiAndLdPointNumber, vatRegNumber, organisationNumber,
                paymentTermsCode, shipmentMethodCode, shippingAgentCode, salesPerson, sourceEtag, creditLimit, blocked,
                cardCustomer);
        }

        private static BuildingBlocks.Domain.CustomerId CustomerId()
        {
            return BuildingBlocks.Domain.CustomerId.Create(Instance<Guid>());
        }

        private static CustomerDetails CustomerDetails()
        {
            return BuildingBlocks.Domain.CustomerDetails.Create(
                CustomerNumber(),
                CustomerAddress.Create(Instance<string>()),
                CustomerBillToName.Create(Instance<string>()),
                CustomerBillToNumber.Create(Instance<string>()),
                CustomerCity.Create(Instance<string>()),
                CustomerDeliveryType.Create(Instance<string>()),
                CustomerIndustry.Create(Instance<string>()),
                BuildingBlocks.Domain.CustomerName.Create(Instance<string>()),
                CustomerPostCode.Create(Instance<string>()),
                CustomerCountryRegion.Create(Instance<string>()));
        }

        private static CreditLimit CreditLimit()
        {
            return Customers.Domain.CreditLimit.Create(Instance<decimal>());
        }

        private static SalesPerson SalesPerson()
        {
            return Customers.Domain.SalesPerson.Create(Instance<string>());
        }

        private static Blocked Blocked()
        {
            return Customers.Domain.Blocked.Create(Instance<string>());
        }

        private static SourceETag SourceEtag()
        {
            return SourceETag.Create(Instance<string>());
        }

        private static BalanceLcy BalanceLcy()
        {
            return Customers.Domain.BalanceLcy.Create(Instance<decimal>());
        }

        private static BalanceDueLcy BalanceDueLcy()
        {
            return Customers.Domain.BalanceDueLcy.Create(Instance<decimal>());
        }

        private static OutstandingOrdersLcy OutstandingOrdersLcy()
        {
            return Customers.Domain.OutstandingOrdersLcy.Create(Instance<decimal>());
        }

        private static NumberNumber NumberNumber()
        {
            return Customers.Domain.NumberNumber.Create(Instance<string>());
        }

        private static OrganisationNumber OrganisationNumber()
        {
            return Customers.Domain.OrganisationNumber.Create(Instance<string>());
        }

        private static PdiAndLdPointNumber PdiAndLdPointNumber()
        {
            return Customers.Domain.PdiAndLdPointNumber.Create(Instance<string>());
        }

        private static VatRegNumber VatRegNumber()
        {
            return Customers.Domain.VatRegNumber.Create(Instance<string>());
        }

        private static PaymentTermsCode PaymentTermsCode()
        {
            return Customers.Domain.PaymentTermsCode.Create(Instance<string>());
        }

        private static ShipmentMethodCode ShipmentMethodCode()
        {
            return Customers.Domain.ShipmentMethodCode.Create(Instance<string>());
        }

        private static ShippingAgentCode ShippingAgentCode()
        {
            return Customers.Domain.ShippingAgentCode.Create(Instance<string>());
        }

        private static CardCustomer CardCustomer()
        {
            return Customers.Domain.CardCustomer.Create(Instance<bool>());
        }
    }
}
