using Insight.OutgoingDeclarations.Application.GetFuelConsumption;
using Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactions;
using Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize;
using Insight.OutgoingDeclarations.Application.GetSustainabilityReport;
using Insight.OutgoingDeclarations.Application.GetSustainabilityReportPdf;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application
{
    public static class OutgoingDeclarationEndpointExtensions
    {
        public static IEndpointRouteBuilder MapOutgoingDeclarationsEndpoints(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapGetOutgoingDeclarationByIdEndpoint();
            endpoint.MapGetSustainabilityReportPdfEndpoint();
            endpoint.MapGetSustainabilityReportEndpoint();
            endpoint.MapGetFuelConsumptionEndpoint();
            endpoint.MapGetFuelConsumptionTransactionsEndpoint();
            endpoint.MapGetFuelConsumptionTransactionsExcelFileEndpoint();
            endpoint.MapGetOutgoingDeclarationsByCustomerIdEndpoint();
            endpoint.MapGetOutgoingDeclarationsEndpoint();
            endpoint.MapGetOutgoingDeclarationsByPageAndPageSizeEndpoint();
            return endpoint;
        }
    }
}
