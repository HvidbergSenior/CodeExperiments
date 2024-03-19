using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Loadings
{
    public class BusinessCentralLoading : BusinessCentralEntity
    {
        [JsonPropertyName("shipmentIdentifier")]
        public long ShipmentIdentifier { get; set; }
        [JsonPropertyName("lineNo")]
        public int LineNumber { get; set; }
        [JsonPropertyName("source")]
        public required string Source { get; set; }
        [JsonPropertyName("loadingTime")]
        public required string LoadingTime { get; set; }
        [JsonPropertyName("plantIdentifier")]
        public int PlantIdentifier { get; set; }
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("productIdentifier")]
        public int ProductIdentifier { get; set; }
        [JsonPropertyName("unitOfMeasure")]
        public required string UnitOfMeasure { get; set; }
        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }
        [JsonPropertyName("loadingReference")]
        public required string LoadingReference { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
        [JsonPropertyName("errorText")]
        public required string ErrorText { get; set; }
        [JsonPropertyName("company")]
        public required string Company { get; set; }
        [JsonPropertyName("correction")]
        public bool Correction { get; set; }
        [JsonPropertyName("loadingDate")]
        public required string LoadingDate { get; set; }
        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }
        [JsonPropertyName("shipmentID")]
        public string? ShipmentID { get; set; }
        [JsonPropertyName("bfeComment")]
        public string? BfeComment { get; set; }
        [JsonPropertyName("purchaseInvoiceNo")]
        public string? PurchaseInvoiceNo { get; set; }
        [JsonPropertyName("systemId")]
        public required Guid SystemId { get; set; }
    }
}