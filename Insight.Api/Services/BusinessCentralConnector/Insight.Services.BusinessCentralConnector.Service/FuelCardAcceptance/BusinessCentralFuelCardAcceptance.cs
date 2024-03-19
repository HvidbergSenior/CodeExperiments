using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.FuelCardAcceptance
{
    public class BusinessCentralFuelCardAcceptance : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("CardName")]
        public string? CardName { get; set; }
        [JsonPropertyName("BFExternalCards_Account")]
        public string? BFExternalCardsAccount { get; set; }
        [JsonPropertyName("BFExternalCards_TokheimCardtype")]
        public string? BFExternalCardsTokheimCardtype { get; set; }
    }
}