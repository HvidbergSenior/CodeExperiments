using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress
{
    public class BusinessCentralFuelCardBiofuelExpress : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("CardNumber")]
        public required string CardNumber { get; set; }
        [JsonPropertyName("BFCard_Country")]
        public required string BFCardCountry { get; set; }
        [JsonPropertyName("BFCard_Account")]
        public required string BFCardAccount { get; set; }
        [JsonPropertyName("BFCard_SubCard")]
        public int BFCardSubCard { get; set; }
        [JsonPropertyName("BFCard_FakturaId")]
        public required string BFCardFakturaId { get; set; }
        [JsonPropertyName("BFCard_CardBlocked")]
        public required string BFCardCardBlocked { get; set; }
        [JsonPropertyName("BFCard_CreditBlocked")]
        public bool BFCardCreditBlocked { get; set; }
        [JsonPropertyName("BFCard_Odometer")]
        public bool BFCardOdometer { get; set; }
        [JsonPropertyName("BFCard_CardType")]
        public required string BFCardCardType { get; set; }
        [JsonPropertyName("SystemCreatedAt")]
        public required DateTime SystemCreatedAt { get; set; }
        public required Guid SystemId { get; set; }
    }
}