using System.Net;
using Insight.BuildingBlocks.Exceptions;

namespace Insight.BuildingBlocks.Validation.WebApi.Errors
{
    public sealed class BusinessError : Error
    {
        private const string DefaultErrorMessage = "Business error";

        public BusinessError()
        {
        }

        public BusinessError(string traceId, string instance) : base(HttpStatusCode.BadRequest, traceId, instance)
        {
            Detail = DefaultErrorMessage;
        }

        public BusinessError(BusinessException exception, string traceId, string instance)
            : base(HttpStatusCode.BadRequest, traceId, instance)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Detail = exception.Message;
        }
    }
}
