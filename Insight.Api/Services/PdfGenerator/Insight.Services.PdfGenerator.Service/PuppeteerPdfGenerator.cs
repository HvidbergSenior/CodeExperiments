using System.Net;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;


namespace Insight.Services.PdfGenerator.Service
{
    public abstract class PuppeteerPdfGenerator : IPdfGenerator
    {
        public abstract Task<Stream> GenerateFromUrl(string targetUrl, string domainName, string accessToken, CancellationToken cancellationToken);

        protected static async Task<Stream> GenerateFromUrl(IBrowser browser, string targetUrl, string domainName, string accessToken, ILogger logger, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("New Page");
                await using var page = await browser.NewPageAsync().ConfigureAwait(false);

                var headers = new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        { nameof(HttpRequestHeader.Authorization), accessToken },
                    };
                await page.SetExtraHttpHeadersAsync(headers).ConfigureAwait(false);

#pragma warning disable CA1307 // Specify StringComparison for clarity
                await page.SetCookieAsync(new CookieParam
                {
                    Name = "accessToken",
                    Value = accessToken.Replace("Bearer ", ""),
                    Domain = domainName,
                    HttpOnly = false,
                    SameSite = SameSite.Lax,
                    Expires = -1
                }).ConfigureAwait(false);
#pragma warning restore CA1307 // Specify StringComparison for clarity

                logger.LogInformation("PuppeteerPdfGenerator.GenerateFromUrl() -> page.goto");
                await page.GoToAsync(targetUrl, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 },
                }).ConfigureAwait(false);

                // The selector is: .SelectorForPDFDownload
                // The . is to indicate className
                await page.WaitForSelectorAsync(".SelectorForPDFDownload");

                await page.EvaluateExpressionHandleAsync("document.fonts.ready");

                return await page.PdfStreamAsync(new PdfOptions
                {
                    Format = PaperFormat.A4,
                    DisplayHeaderFooter = true,
                    PreferCSSPageSize = true,
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on GenerateFromUrl: {ExceptionString}", ex.ToString());
                throw;
            }
        }
    }
}