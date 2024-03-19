namespace Insight.IncomingDeclarations.Application
{
    public static class IncomingDeclarationsEndpointUrls
    {
        public const string GET_INCOMING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT = "/api/incomingdeclarations/pagination";
        public const string UPLOAD_INCOMING_DECLARATION_ENDPOINT = "/api/incomingdeclarations/upload";
        public const string APPROVE_INCOMING_DECLARATION_UPLOAD_ENDPOINT = "/api/incomingdeclarations/approve";
        public const string RECONCILE_INCOMING_DECLARATION_ENDPOINT = "/api/incomingdeclarations/reconcile";
        public const string UPDATE_INCOMING_DECLARATION_ENDPOINT = "/api/incomingdeclarations/{id}";
        public const string GET_INCOMING_DECLARATIONS_RECONCILED_ENDPOINT = "/api/incomingdeclarations/reconciled";
        public const string GET_INCOMING_DECLARATION_BY_ID_ENDPOINT = "/api/incomingdeclarations/{id}";
        public const string CANCEL_INCOMING_DECLARATIONS_BY_UPLOAD_ID_ENDPOINT = "/api/incomingdeclarations/cancel/{uploadId}";
    }
}
