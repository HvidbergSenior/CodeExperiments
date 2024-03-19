using System.IO;
using System.Threading.Tasks;

namespace Insight.Services.PdfGenerator.Service
{
    public interface IPdfGenerator
    {
        Task<Stream> GenerateFromUrl(string targetUrl, string domainName, string accessToken, CancellationToken cancellationToken = default);
    }
}