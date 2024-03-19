using System.ComponentModel.DataAnnotations;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile
{
    public class GetFuelConsumptionTransactionsExcelFileRequest
    {
        [Required(ErrorMessage = "ProductNames is required")]
        public required ProductNameEnumeration[]? ProductNames { get; set; }
        [Required(ErrorMessage = "CustomerIds is required")]
        public Guid[]? CustomerIds { get; set; }
        [Required(ErrorMessage = "CustomerNumbers is required")]
        public string[]? CustomerNumbers { get; set; }
        [Required(ErrorMessage = "MaxColumns is required")]
        public required int? MaxColumns { get; set; }
        [Required(ErrorMessage = "DateTo is required")]
        public required  DateOnly? DateTo { get; set; }
        [Required(ErrorMessage = "DateFrom is required")]
        public required DateOnly? DateFrom { get; set; }
    }

}