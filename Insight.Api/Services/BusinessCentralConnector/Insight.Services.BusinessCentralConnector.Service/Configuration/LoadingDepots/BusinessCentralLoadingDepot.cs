using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Configuration.LoadingDepots
{
    public class BusinessCentralLoadingDepot : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("PlantIdentifier")]
        public int PlantIdentifier { get; set; }
        [JsonPropertyName("Company")]
        public required string Company { get; set; }
        [JsonPropertyName("Description")]
        public required string Description { get; set; }
        [JsonPropertyName("ISCCStorage")]
        public required string ISCCStorage { get; set; }
        [JsonPropertyName("Vendor")]
        public required string Vendor { get; set; }
        [JsonPropertyName("Name")]
        public required string Name { get; set; }
        [JsonPropertyName("Address")]
        public required string Address { get; set; }
        [JsonPropertyName("Address2")]
        public required string Address2 { get; set; }
        [JsonPropertyName("PostCode")]
        public required string PostCode { get; set; }
        [JsonPropertyName("City")]
        public required string City { get; set; }
        [JsonPropertyName("Country")]
        public required string Country { get; set; }
        [JsonPropertyName("SystemId")]
        public required Guid SystemId { get; set; }
    }
}
