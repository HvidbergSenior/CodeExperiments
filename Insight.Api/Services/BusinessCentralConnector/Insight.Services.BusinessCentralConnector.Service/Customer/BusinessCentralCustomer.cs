using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Customer
{
    public class BusinessCentralCustomer : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("No")]
        public required string Number { get; set; }
        [JsonPropertyName("No_Customer")]
        public required string CustomerNumber { get; set; }
        [JsonPropertyName("Name_Customer")]
        public required string CustomerName { get; set; }
        [JsonPropertyName("Address_Customer")]
        public required string CustomerAddress { get; set; }
        [JsonPropertyName("City_Customer")]
        public required string CustomerCity { get; set; }
        [JsonPropertyName("Post_Code_Customer")]
        public required string CustomerPostCode { get; set; }
        [JsonPropertyName("CountryRegion_Customer")]
        public required string CustomerCountryRegion { get; set; }
        [JsonPropertyName("BillToNo_Customer")]
        public required string CustomerBillToNumber { get; set; }
        [JsonPropertyName("DeliveryType_Customer")]
        public required string CustomerDeliveryType { get; set; }
        [JsonPropertyName("Industry_Customer")]
        public required string CustomerIndustry { get; set; }
        [JsonPropertyName("BillToName_Customer")]
        public required string CustomerBillToName { get; set; }
        [JsonPropertyName("PaymentTerms_Code")]
        public required string PaymentTermsCode { get; set; }
        [JsonPropertyName("Credit_Limit")]
        public decimal CreditLimit { get; set; }
        [JsonPropertyName("BalanceLCY")]
        public decimal BalanceLCY { get; set; }
        [JsonPropertyName("BalanceDueLCY")]
        public decimal BalanceDueLCY { get; set; }
        [JsonPropertyName("Blocked")]
        public required string Blocked { get; set; }
        [JsonPropertyName("OutstandingOrdersLCY")]
        public decimal OutstandingOrdersLCY { get; set; }
        [JsonPropertyName("SalesPerson")]
        public required string SalesPerson { get; set; }
        [JsonPropertyName("CardCustomer")]
        public bool CardCustomer { get; set; }
        [JsonPropertyName("ShipmentMethodCode")]
        public required string ShipmentMethodCode { get; set; }
        [JsonPropertyName("ShippingAgentCode")]
        public required string ShippingAgentCode { get; set; }
        [JsonPropertyName("PDI_LDPointNr")]
        public required string PDIAndLDPointNumber { get; set; }
        [JsonPropertyName("VATRegNo")]
        public required string VATRegNumber { get; set; }
        [JsonPropertyName("OrganisationNo")]
        public required string OrganisationNumber { get; set; }
    }
}