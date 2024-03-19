namespace Insight.Services.AllocationEngine.Application
{
    public static class AllocationEngineEndpointUrls
    {
        public const string GET_ALLOCATION_SUGGESTIONS_ENDPOINT = "/api/allocations/suggestions";
        public const string POST_MANUAL_ALLOCATION_ENDPOINT = "/api/allocations/manual";
        public const string POST_AUTOMATIC_ALLOCATION_ENDPOINT = "/api/allocations/automatic";
        public const string GET_ALLOCATIONS_ENDPOINT = "/api/allocations";
        public const string LOCK_ALLOCATIONS_ENDPOINT = "/api/allocations/lock";
        public const string UNLOCK_ALLOCATIONS_ENDPOINT = "/api/allocations/unlock";
        public const string PUBLISH_ALLOCATIONS_ENDPOINT = "/api/allocations/publish";
        public const string CLEAR_ALLOCATIONS_ENDPOINT = "/api/allocations/clear";
        public const string GET_ALLOCATION_BY_ID_ENDPOINT = "/api/allocations/{id}";

    }
}
