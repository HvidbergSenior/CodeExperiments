using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet
{
    public class BusinessCentralFuelTransactionsTapnet : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("Processed")]
        public string? Processed { get; set; }
        [JsonPropertyName("TransDate")]
        public required string TransDate { get; set; }
        [JsonPropertyName("TransTime")]
        public required string TransTime { get; set; }
        [JsonPropertyName("TapNetTrans_RegistrationNumber")]
        public string? TapNetTransRegistrationNumber { get; set; }
        [JsonPropertyName("SeqNumber")]
        public int SeqNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_Processed")]
        public string? TapNetTransmissionsProcessed { get; set; }
        [JsonPropertyName("TapNetTransmissions_TransDate")]
        public string? TapNetTransmissionsTransDate { get; set; }
        [JsonPropertyName("TapNetTransmissions_Time")]
        public string? TapNetTransmissionsTime { get; set; }
        [JsonPropertyName("TapNetTransmissions_SeqNumber")]
        public int TapNetTransmissionsSeqNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_BatchNum")]
        public int TapNetTransmissionsBatchNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_ReportRef")]
        public string? TapNetTransmissionsReportRef { get; set; }
        [JsonPropertyName("ReportRef")]
        public string? ReportRef { get; set; }
        [JsonPropertyName("TapNetTrans_StationNumber")]
        public required string TapNetTransStationNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_CountrySold")]
        public string? TapNetTransmissionsCountrySold { get; set; }
        [JsonPropertyName("TapNetTransmissions_POSTicket")]
        public int TapNetTransmissionsPostTicket { get; set; }
        [JsonPropertyName("TapNetTransmissions_CardNumber")]
        public required string TapNetTransmissionsCardNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_CountryCust")]
        public string? TapNetTransmissionsCountryCustomer { get; set; }
        [JsonPropertyName("TapNetTransmissions_Odometer")]
        public int TapNetTransmissionsOdometer { get; set; }
        [JsonPropertyName("TapNetTransmissions_AuthCode")]
        public required string TapNetTransmissionsAuthCode { get; set; }
        [JsonPropertyName("TapNetTransmissions_CardStatus")]
        public int TapNetTransmissionsCardStatus { get; set; }
        [JsonPropertyName("TapNetTransmissions_Currency")]
        public required string TapNetTransmissionsCurrency { get; set; }
        [JsonPropertyName("TapNetTransmissions_Pump")]
        public required int TapNetTransmissionsPump { get; set; }
        [JsonPropertyName("TapNetTransmissions_ItemNumber")]
        public required string TapNetTransmissionsItemNumber { get; set; }
        [JsonPropertyName("TapNetTransmissions_Qty")]
        public decimal TapNetTransmissionsQuantity { get; set; }
        [JsonPropertyName("TapNetTransmissions_PricePump")]
        public decimal TapNetTransmissionsPricePump { get; set; }
        [JsonPropertyName("TapNetTrans_AmountInclVat")]
        public decimal TapNetTransAmountInclVat { get; set; }
        [JsonPropertyName("TapNetTransmissions_VatRate")]
        public int TapNetTransmissionsVatRate { get; set; }
        [JsonPropertyName("TapNetTransmissions_VatAmount")]
        public decimal TapNetTransmissionsVatAmount { get; set; }
        [JsonPropertyName("TapNetTransmissions_ErrorText")]
        public required string TapNetTransmissionsErrorText { get; set; }
        [JsonPropertyName("TapNetTransmissions_CardType")]
        public required string TapNetTransmissionsCardType { get; set; }
        [JsonPropertyName("TapNetTransmissions_DriverId")]
        public required string TapNetTransmissionsDriverId { get; set; }
        [JsonPropertyName("TapNetTrans_CardNumber2")]
        public required string TapNetTransCardNumber2 { get; set; }
        [JsonPropertyName("TapNet_CustNo")]
        public required string TapNetCustomerNumber { get; set; }
        [JsonPropertyName("SystemCreatedAt")]
        public required DateTime SystemCreatedAt { get; set; }
        public required Guid SystemId { get; set; }
    }
}