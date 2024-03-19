using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsDialog
{
    public class BusinessCentralFuelTransactionsDialog : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("Processed")]
        public string? Processed { get; set; }
        [JsonPropertyName("SeqNumber")]
        public int SeqNumber { get; set; }
        [JsonPropertyName("ReportRef")]
        public string? ReportRef { get; set; }
        [JsonPropertyName("TransDate")]
        public required string TransDate { get; set; }
        [JsonPropertyName("TransTime")]
        public required string TransTime { get; set; }
        [JsonPropertyName("diaLOGtransactions_Processed")]
        public string? DialogTransactionsProcessed { get; set; }
        [JsonPropertyName("diaLOGtransactions_TransDate")]
        public string? DialogTransactionsTransDate { get; set; }
        [JsonPropertyName("diaLOGtransactions_Time")]
        public string? diaLOGtransactionsTime { get; set; }
        [JsonPropertyName("diaLOGtransactions_SeqNumber")]
        public int DialogTransactionsSeqNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_BatchNum")]
        public int DialogTransactionsBatchNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_ReportRef")]
        public string? DialogTransactionsReportRef { get; set; }
        [JsonPropertyName("diaLOGtransactions_StationNumber")]
        public required string DialogTransactionsStationNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_CardNumber")]
        public required string DialogTransactionsCardNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_Odometer")]
        public int DialogTransactionsOdometer { get; set; }
        [JsonPropertyName("diaLOGtransactions_Currency")]
        public string? DialogTransactionsCurrency { get; set; }
        [JsonPropertyName("diaLOGtransactions_ItemNumber")]
        public required string DialogTransactionsItemNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_Qty")]
        public required decimal DialogTransactionsQuantity { get; set; }
        [JsonPropertyName("diaLOGtransactions_VatRate")]
        public int DialogTransactionsVatRate { get; set; }
        [JsonPropertyName("diaLOGtransactions_ErrorText")]
        public string? DialogTransactionsErrorText { get; set; }
        [JsonPropertyName("diaLOGtransactions_DriverCardNumber")]
        public required string DialogTransactionsDriverCardNumber { get; set; }
        [JsonPropertyName("diaLOGtransactions_CustNo")]
        public required string DialogTransactionsCustomerNumber { get; set; }
        [JsonPropertyName("SystemCreatedAt")]
        public required DateTime SystemCreatedAt { get; set; }
        public required Guid SystemId { get; set; }
    }
}