using Microsoft.AspNetCore.Http;

namespace Insight.Services.PdfGenerator.Service
{
    public class UrlBuilder
    {
        public static Uri BuildSummaryUrl(Guid institutionId, Guid analysisId, HttpRequest request)
        {
            UriBuilder response = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? -1, // a value of -1, this indicates that the default port value for the protocol scheme will be used to connect to the host
                Path = $"institutions/{institutionId}/analysis-study/{analysisId}/summary",
            };

            return response.Uri;
        }

        public static Uri BuildSummaryPdfUrl(Guid institutionId, Guid analysisId, HttpRequest request)
        {
            UriBuilder response = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? -1, // a value of -1, this indicates that the default port value for the protocol scheme will be used to connect to the host
                Path = $"resources/institutions/{institutionId}/analysisStudies/{analysisId}/summaryPDF",
            };

            return response.Uri;
        }
    }
}
