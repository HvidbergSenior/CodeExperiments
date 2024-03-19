using System.Net;

namespace Insight.BuildingBlocks.Validation.WebApi.Errors
{
    public sealed class ExceptionError : Error
    {
        public ExceptionError(HttpStatusCode httpStatusCode, string traceId, string instance) : base(httpStatusCode, traceId, instance)
        {
        }
    }
}
