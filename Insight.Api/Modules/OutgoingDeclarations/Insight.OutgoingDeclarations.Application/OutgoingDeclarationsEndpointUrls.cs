namespace Insight.OutgoingDeclarations.Application
{
    public static class OutgoingDeclarationsEndpointUrls
    {
        public const string GET_OUTGOING_DECLARATION_BY_ID_ENDPOINT = "/api/outgoingdeclarations/{id}";
        public const string GET_SUSTAINABILITY_REPORTPDF_ENDPOINT = "/api/outgoingdeclarations/sustainabilityreportpdf";
        public const string GET_SUSTAINABILITY_REPORT_ENDPOINT = "/api/outgoingdeclarations/sustainabilityreport";
        public const string GET_FUELCONSUMPTION_ENDPOINT = "/api/outgoingdeclarations/fuelconsumption";
        public const string GET_FUELCONSUMPTION_TRANSACTIONS_ENDPOINT = "/api/outgoingdeclarations/fuelconsumptiontransactions";
        public const string GET_FUELCONSUMPTION_TRANSACTIONS_EXCEL_FILE_ENDPOINT = "/api/outgoingdeclarations/fuelconsumptiontransactionsexcelfile";
        public const string GET_OUTGOING_DECLARATIONS_BY_CUSTOMER_ID_ENDPOINT = "/api/outgoingdeclarations/customer/{customerId}";
        public const string GET_OUTGOING_DECLARATIONS_ENDPOINT = "/api/outgoingdeclarations";
        public const string GET_OUTGOING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT = "/api/outgoingdeclarations/pagination";
    }
}
