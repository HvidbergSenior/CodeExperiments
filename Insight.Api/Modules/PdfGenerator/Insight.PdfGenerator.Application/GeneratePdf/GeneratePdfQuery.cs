using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.Services.PdfGenerator.Service;
using Microsoft.Extensions.Logging;

namespace Insight.PdfGenerator.Application.GeneratePdf
{
    public sealed class GeneratePdfQuery : IQuery<Stream>
    {
        internal string TargetUrl { get; set; }
        internal string DomainName { get; set; }
        internal string AccessToken{ get; set; }
        
        private GeneratePdfQuery(
            string targetUrl, 
            string domainName,
            string accessToken)
        {
            TargetUrl = targetUrl;
            DomainName = domainName;
            AccessToken = accessToken;
        }

        public static GeneratePdfQuery Create(
            string targetUrl, 
            string domainName,
            string accessToken)
        {
            return new GeneratePdfQuery(
                 targetUrl, 
                 domainName,
                 accessToken);
        }
    }

    internal class GeneratePdfQueryHandler : IQueryHandler<GeneratePdfQuery, Stream>
    {
        private readonly IPdfGenerator pdfGenerator;
        private readonly ILogger<GeneratePdfQueryHandler> logger;

        public GeneratePdfQueryHandler(IPdfGenerator pdfGenerator
            , ILogger<GeneratePdfQueryHandler> logger)
        {
            this.pdfGenerator = pdfGenerator;
            this.logger = logger;
        }

        public async Task<Stream> Handle(GeneratePdfQuery request, CancellationToken cancellationToken)
        {
            return await pdfGenerator.GenerateFromUrl(
                request.TargetUrl,
                request.DomainName,
                request.AccessToken,
                cancellationToken
                ).ConfigureAwait(false);
        }
    }
    internal class GeneratePdfQueryAuthorizer : IAuthorizer<GeneratePdfQuery>
    {
        private readonly IExecutionContext executionContext;
        
        public GeneratePdfQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GeneratePdfQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}