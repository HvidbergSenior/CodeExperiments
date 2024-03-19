using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.RawMaterial
{
    public class BusinessCentralRawMaterial : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")] 
        public required string Etag { get; set; }
        [JsonPropertyName("CustomerNo")]
        public required string CustomerNumber { get; set; }
        [JsonPropertyName("Feedstock")] 
        public required string Feedstock { get; set; }
        [JsonPropertyName("AllowedfeedstocksIncludeExclude")] 
        public required string IncludeExclude { get; set; }
    }
}