using System.Net;
using System.Threading;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.FuelTransactions.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.PdfGenerator.Application.GeneratePdf;

public static class GeneratePdfEndpoint
{
    public static void MapGeneratePdfEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet(GeneratePdfEndpointUrls.GENERATE_PDF_ENDPOINT, async (
            string targetUrl,
            IQueryBus queryBus,
            CancellationToken cancellationToken,
            HttpRequest request, IExecutionContext executionContext) =>
            {
                var accessToken = request.Headers[HttpRequestHeader.Authorization.ToString()].Single() ?? (request.Cookies["accessToken"] ?? throw new MissingMemberException());
                var uriAddress = new Uri(targetUrl);
                var domainName = uriAddress.Host;
                
                var customerPermissions = await executionContext.GetCustomersPermissionsAsync(); // Use for customer filtered data.

                var query = GeneratePdfQuery.Create(
                    targetUrl,
                    domainName,
                    accessToken);
                var results = await queryBus.Send<GeneratePdfQuery, Stream>(query, cancellationToken).ConfigureAwait(false);

                return Results.File(results, "application/octet-stream", "document.pdf");
            })
        .RequireAuthorization()
        .WithName("generatepdf")
        .WithTags("GeneratePDF");
    }
}
