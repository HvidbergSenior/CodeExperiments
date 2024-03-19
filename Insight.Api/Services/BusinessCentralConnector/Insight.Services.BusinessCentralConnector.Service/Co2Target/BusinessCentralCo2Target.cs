using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Co2Target
{
    public class BusinessCentralCo2Target : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")] 
        public string? Etag { get; set; }
        [JsonPropertyName("CustomerNo")] 
        public string? CustomerNumber { get; set; }
        [JsonPropertyName("CO2target_Co2Taget")] 
        public decimal Co2Target { get; set; }
    }
}