using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Suppliers
{
    public class BusinessCentralSuppliers : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("No")]
        public required string Number { get; set; }
        [JsonPropertyName("Name_Vendor")]
        public required string VendorName { get; set; }
        [JsonPropertyName("Address_Vendor")]
        public required string VendorAddress { get; set; }
        [JsonPropertyName("City_Vendor")]
        public required string VendorCity { get; set; }
        [JsonPropertyName("Post_Code_Vendor")]
        public required string VendorPostCode { get; set; }
        [JsonPropertyName("CountryRegion_Vendor")]
        public required string VendorCountryRegion { get; set; }
    }
}