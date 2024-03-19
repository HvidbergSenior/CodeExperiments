using System.ComponentModel.DataAnnotations;
using Insight.OutgoingDeclarations.Application.Helpers;

namespace Insight.OutgoingDeclarations.Application.GetSustainabilityReport
{
    public sealed class GetSustainabilityReportResponse
    {
        [Required]
        public required Feedstock[] Feedstocks { get; set; }
        [Required]
        public required Country[] Countries { get; set; }
        [Required]
        public required Progress Progress { get; set; }
        [Required]
        public required ProductSpecificationItem[] ProductSpecificationItems { get; set; }
        [Required]
        public required Emissionsstats EmissionsStats { get; set; }
    }
}