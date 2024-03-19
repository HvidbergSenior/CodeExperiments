using Insight.BuildingBlocks.Integration;

namespace Insight.Services.BusinessCentralConnector.Integration
{
    public class CustomerCreatedOrUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid CompanyId { get; set; }
        public string SourceETag { get; set; }
        public string Number { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPostCode { get; set; }
        public string CustomerCountryRegion { get; set; }
        public string CustomerBillToNumber { get; set; }
        public string CustomerDeliveryType { get; set; }
        public string CustomerIndustry { get; set; }
        public string CustomerBillToName { get; set; }
        public string PaymentTermsCode { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal BalanceLcy { get; set; }
        public decimal BalanceDueLcy { get; set; }
        public string Blocked { get; set; }
        public decimal OutstandingOrdersLcy { get; set; }
        public string SalesPerson { get; set; }
        public bool CardCustomer { get; set; }
        public string ShipmentMethodCode { get; set; }
        public string ShippingAgentCode { get; set; }
        public string PdiAndLdPointNumber { get; set; }
        public string VatRegNumber { get; set; }
        public string OrganisationNumber { get; set; }

        private CustomerCreatedOrUpdatedIntegrationEvent() : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId = Guid.Empty;
            SourceETag = string.Empty;
            Number = string.Empty;
            CustomerNumber = string.Empty;
            CustomerName = string.Empty;
            CustomerAddress = string.Empty;
            CustomerCity = string.Empty;
            CustomerPostCode = string.Empty;
            CustomerCountryRegion = string.Empty;
            CustomerBillToNumber = string.Empty;
            CustomerDeliveryType = string.Empty;
            CustomerIndustry = string.Empty;
            CustomerBillToName = string.Empty;
            PaymentTermsCode = string.Empty;
            CreditLimit = default;
            BalanceLcy = default;
            BalanceDueLcy = default;
            Blocked = string.Empty;
            OutstandingOrdersLcy = default;
            SalesPerson = string.Empty;
            CardCustomer = default;
            ShipmentMethodCode = string.Empty;
            ShippingAgentCode = string.Empty;
            PdiAndLdPointNumber = string.Empty;
            VatRegNumber = string.Empty;
            OrganisationNumber = string.Empty;
        }

        private CustomerCreatedOrUpdatedIntegrationEvent(
            Guid companyId,
            string sourceETag,
            string number,
            string customerNumber,
            string customerName,
            string customerAddress,
            string customerCity,
            string customerPostCode,
            string customerCountryRegion,
            string customerBillToNumber,
            string customerDeliveryType,
            string customerIndustry,
            string customerBillToName,
            string paymentTermsCode,
            decimal creditLimit,
            bool cardCustomer,
            string shipmentMethodCode,
            string shippingAgentCode,
            string organisationNumber,
            decimal balanceLcy,
            decimal balanceDueLcy,
            string blocked,
            decimal outstandingOrdersLcy,
            string salesPerson,
            string pdiAndLdPointNumber,
            string vatRegNumber
        ) : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId = companyId;
            SourceETag = sourceETag;
            Number = number;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerCity = customerCity;
            CustomerPostCode = customerPostCode;
            CustomerCountryRegion = customerCountryRegion;
            CustomerBillToNumber = customerBillToNumber;
            CustomerDeliveryType = customerDeliveryType;
            CustomerIndustry = customerIndustry;
            CustomerBillToName = customerBillToName;
            PaymentTermsCode = paymentTermsCode;
            CreditLimit = creditLimit;
            CardCustomer = cardCustomer;
            ShipmentMethodCode = shipmentMethodCode;
            ShippingAgentCode = shippingAgentCode;
            OrganisationNumber = organisationNumber;
            BalanceLcy = balanceLcy;
            BalanceDueLcy = balanceDueLcy;
            Blocked = blocked;
            OutstandingOrdersLcy = outstandingOrdersLcy;
            SalesPerson = salesPerson;
            PdiAndLdPointNumber = pdiAndLdPointNumber;
            VatRegNumber = vatRegNumber;
        }

        public static CustomerCreatedOrUpdatedIntegrationEvent Create(
            Guid companyId,
            string sourceEtag,
            string number,
            string customerNumber,
            string customerName,
            string customerAddress,
            string customerCity,
            string customerPostCode,
            string customerCountryRegion,
            string customerBillToNumber,
            string customerDeliveryType,
            string customerIndustry,
            string customerBillToName,
            string paymentTermsCode,
            decimal creditLimit,
            bool cardCustomer,
            string shipmentMethodCode,
            string shippingAgentCode,
            string organisationNumber,
            decimal balanceLcy,
            decimal balanceDueLcy,
            string blocked,
            decimal outstandingOrdersLcy,
            string salesPerson,
            string pdiAndLdPointNumber,
            string vatRegNumber)
        {
            return new CustomerCreatedOrUpdatedIntegrationEvent(
                companyId,
                sourceEtag,
                number,
                customerNumber,
                customerName,
                customerAddress,
                customerCity,
                customerPostCode,
                customerCountryRegion,
                customerBillToNumber,
                customerDeliveryType,
                customerIndustry,
                customerBillToName,
                paymentTermsCode,
                creditLimit,
                cardCustomer,
                shipmentMethodCode,
                shippingAgentCode,
                organisationNumber,
                balanceLcy,
                balanceDueLcy,
                blocked,
                outstandingOrdersLcy,
                salesPerson,
                pdiAndLdPointNumber,
                vatRegNumber
            );
        }
    }
}