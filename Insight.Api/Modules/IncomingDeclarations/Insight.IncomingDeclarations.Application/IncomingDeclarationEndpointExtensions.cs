using Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads;
using Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationById;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsReconciled;
using Insight.IncomingDeclarations.Application.Reconciliation;
using Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration;
using Microsoft.AspNetCore.Routing;

namespace Insight.IncomingDeclarations.Application
{
    public static class IncomingDeclarationEndpointExtensions
    {
        public static IEndpointRouteBuilder MapGetIncomingDeclarationsEndpoints(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapGetIncomingDeclarationsByPageAndPageSizeEndpoint();
            endpoint.MapGetIncomingDeclarationsReconciledEndpoint();
            endpoint.MapApproveIncomingDeclarationUploadEndpoint();
            endpoint.MapReconcileIncomingDeclarationsEndpoint();
            endpoint.MapUpdateIncomingDeclarationEndpoint();
            endpoint.MapGetIncomingDeclarationByIdEndpoint();
            endpoint.MapCancelIncomingDeclarationsByUploadIdEndpoint();
            return endpoint;
        }
    }
}