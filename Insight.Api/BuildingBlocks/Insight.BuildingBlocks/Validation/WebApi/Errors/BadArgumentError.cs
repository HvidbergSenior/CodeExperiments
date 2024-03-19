using System.Net;

namespace Insight.BuildingBlocks.Validation.WebApi.Errors
{
    public sealed class BadArgumentError : Error
    {
        public BadArgumentError(HttpStatusCode _httpStatusCode, string traceId, string instance) : base(_httpStatusCode, traceId, instance)
        {
        }
    }
}
