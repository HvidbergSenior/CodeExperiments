using System.Globalization;
using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Itemledger
{
    public class BusinessCentralItemLedger : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("EntryNo")]
        public int EntryNumber { get; set; }
        [JsonPropertyName("SourceType")]
        public required string SourceType { get; set; }
        [JsonPropertyName("SourceNo")]
        public required string SourceNumber { get; set; }
        [JsonPropertyName("DocumentNo")]
        public required string DocumentNumber { get; set; }
        [JsonPropertyName("PostingDate")]
        public required string PostingDate { get; set; }
        [JsonPropertyName("EntryType")]
        public required string EntryType { get; set; }
        [JsonPropertyName("Quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("SalesAmountActual")]
        public decimal SalesAmountActual { get; set; }
        [JsonPropertyName("LocationCode")]
        public required string LocationCode { get; set; }
        [JsonPropertyName("ItemNo")]
        public required string ItemNumber { get; set; }
        [JsonPropertyName("PO_number")]
        public required string PONumber { get; set; }
        [JsonPropertyName("PDI_ShipmentIdentifier")]
        public long PDIShipmentIdentifier { get; set; }
        public required string ShipmentDate { get; set; }
        public required string TransTime { get; set; }
        public required Guid SystemId { get; set; }
        public required DateTime SystemCreatedAt { get; set; }
    }
}