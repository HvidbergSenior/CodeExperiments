using System.ComponentModel.DataAnnotations;
using Insight.OutgoingDeclarations.Application.Helpers;

namespace Insight.OutgoingDeclarations.Application.GetSustainabilityReportPdf
{
    public sealed class GetSustainabilityReportPdfResponse
    {
        [Required]
        public required ConsumptionStats ConsumptionStats { get; set; }
        [Required]
        public required Emissionsstats EmissionsStats { get; set; }
        [Required]
        public required ConsumptionPerProduct ConsumptionPerProduct { get; set; }
        [Required]
        public required ConsumptionDevelopment ConsumptionDevelopment { get; set; }
        [Required]
        public required Progress Progress { get; set; }
        [Required]
        public required Feedstock[] Feedstocks { get; set; }
        [Required]
        public required Country[] Countries { get; set; }
        [Required]
        public required ProductSpecificationItem[] ProductSpecificationItems { get; set; }
        [Required]
        public required IReadOnlyList<PdfReportPosResponse> PdfReportPosResponses { get; set; }

    }
}