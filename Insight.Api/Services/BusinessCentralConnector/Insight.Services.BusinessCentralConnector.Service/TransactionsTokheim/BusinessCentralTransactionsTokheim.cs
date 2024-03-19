using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim
{
    public class BusinessCentralTransactionsTokheim : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("processed")]
        public string? Processed { get; set; }
        [JsonPropertyName("TransDate")]
        public required string TransDate { get; set; }
        [JsonPropertyName("TransTime")]
        public required string TransTime { get; set; }
        [JsonPropertyName("RegistrationNumber")]
        public required string RegistrationNumber { get; set; }
        [JsonPropertyName("seqNumber")]
        public int SequenceNumber { get; set; }
        [JsonPropertyName("Trans_Date")]
        public string? TransDate2 { get; set; }
        [JsonPropertyName("CountrySold")]
        public string? CountrySold { get; set; }
        [JsonPropertyName("StationNumber")]
        public required string StationNumber { get; set; }
        [JsonPropertyName("ItemNumber")]
        public required string ItemNumber { get; set; }
        [JsonPropertyName("Liter_BF")]
        public required decimal LiterBF { get; set; }
        [JsonPropertyName("KM_BF")]
        public required int KMBF { get; set; }
        [JsonPropertyName("Currency")]
        public string? Currency { get; set; }
        [JsonPropertyName("CardType")]
        public required string CardType { get; set; }
        [JsonPropertyName("CardTypeName")]
        public required string CardTypeName { get; set; }
        [JsonPropertyName("DriverCardNumber")]
        public required string DriverCardNumber { get; set; }
        [JsonPropertyName("CardNumber")]
        public required string CardNumber { get; set; }
        [JsonPropertyName("Account")]
        public required string Account { get; set; }
        [JsonPropertyName("SystemCreatedAt")]
        public required DateTime SystemCreatedAt { get; set; }
        public required Guid SystemId { get; set; }
    }
}