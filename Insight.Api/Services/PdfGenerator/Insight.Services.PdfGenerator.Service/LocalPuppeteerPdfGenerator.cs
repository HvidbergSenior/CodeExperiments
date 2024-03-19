using Microsoft.Extensions.Logging;
using PuppeteerSharp;


namespace Insight.Services.PdfGenerator.Service

{
    public class LocalPuppeteerPdfGenerator : PuppeteerPdfGenerator
    {
        private readonly ILogger<LocalPuppeteerPdfGenerator> _logger;
        private readonly bool ignoreHttpsErrors;
        public LocalPuppeteerPdfGenerator(IPdfGeneratorConfig options,ILogger<LocalPuppeteerPdfGenerator> logger)
        {
            _logger = logger;
            ignoreHttpsErrors = options.IgnoreHttpsErrors;
        }

        public override async Task<Stream> GenerateFromUrl(string targetUrl, string domainName, string accessToken, CancellationToken cancellationToken)
        {
            try
            {
                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync().ConfigureAwait(false);
                _logger.LogInformation("LocalPuppeteerPdfGenerator.GenerateFromUrl() -> Downloaded browser, Launching.");
                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, IgnoreHTTPSErrors = ignoreHttpsErrors});

                var stream = await GenerateFromUrl(browser, targetUrl, domainName, accessToken,  _logger, cancellationToken);
                _logger.LogInformation("LocalPuppeteerPdfGenerator.GenerateFromUrl() -> Generated stream, disposing browser");
                await browser.DisposeAsync().ConfigureAwait(false);
                return stream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception on GenerateFromUrl: {ExceptionString}", ex.ToString());
                throw;
            }
        }
    }
}