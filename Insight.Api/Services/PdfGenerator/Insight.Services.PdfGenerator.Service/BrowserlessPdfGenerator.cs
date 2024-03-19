using Microsoft.Extensions.Logging;
using PuppeteerSharp;
namespace Insight.Services.PdfGenerator.Service
{
    public class BrowserlessPdfGenerator : PuppeteerPdfGenerator
    {
        private readonly ILogger<BrowserlessPdfGenerator> _logger;
        private readonly string browserWsEndpoint;
        private readonly int timeout;
        private readonly bool ignoreHttpsErrors;

        public BrowserlessPdfGenerator(IPdfGeneratorConfig options, ILogger<BrowserlessPdfGenerator> logger)
        {
            browserWsEndpoint = options.BrowserWsEndpoint;
            timeout = options.Timeout;
            ignoreHttpsErrors = options.IgnoreHttpsErrors;
            _logger = logger;
        }

        public override async Task<Stream> GenerateFromUrl(string targetUrl, string domainName, string accessToken, CancellationToken cancellationToken)
        {
            try
            {
                var options = new ConnectOptions()
                {
                    BrowserWSEndpoint = $"{browserWsEndpoint}",
                    IgnoreHTTPSErrors = ignoreHttpsErrors
                };

                var tokenCancellationSource = new CancellationTokenSource(timeout); // Cancel after set amount of time.
                var browser = await Puppeteer.ConnectAsync(options).WithCancellation(tokenCancellationSource.Token).ConfigureAwait(false);

                var stream = await GenerateFromUrl(browser, targetUrl, domainName, accessToken, _logger, cancellationToken).ConfigureAwait(false);
                await browser.DisposeAsync().ConfigureAwait(false);
                return stream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF");
                throw;
            }
        }
    }
}