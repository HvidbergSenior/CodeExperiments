using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile;

public class GetFuelConsumptionTransactionsExcelFileResponse
{
    [Required]
    public required Stream Data { get; set; }
    [Required]
    public string FileName { get; set; } = "";
    [Required]
    public string ContentType { get; set; } = "";
}