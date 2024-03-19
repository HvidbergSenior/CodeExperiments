using System.ComponentModel.DataAnnotations;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.Helpers;

public sealed class ConsumptionPerProduct
{
    [Required]
    public required IReadOnlyList<FuelConsumptionNameValuePair> Data { get; set; }
}

public sealed class FuelConsumptionNameValuePair
{
    [Required]
    public ProductNameEnumeration ProductNameEnumeration { get; set; }
    [Required]
    public decimal Value { get; set; }
    
    public FuelConsumptionNameValuePair(ProductNameEnumeration productNameEnumeration, decimal value)
    {
        ProductNameEnumeration = productNameEnumeration;
        Value = value;
    }
}
public sealed class ConsumptionDevelopment
{
    [Required]
    public required IReadOnlyList<FuelConsumptionSeries> Series { get; set; }
    [Required]
    public required IReadOnlyList<string> Categories { get; set; }
}

public sealed class FuelConsumptionSeries
{
    [Required]
    public required ProductNameEnumeration ProductNameEnumeration { get; set; }
    [Required]
    public required IReadOnlyList<int> Data { get; set; }

    public FuelConsumptionSeries()
    {
    }
}

public sealed class ConsumptionStats
{
    [Required]
    public required IReadOnlyList<int> Data { get; set; }
    [Required]
    public required IReadOnlyList<string> GeneralFuelTypes { get; set; }
    [Required]
    public required int TotalConsumptionFossilFuels { get; set; }
    [Required]
    public required int TotalConsumptionRenewableFuels { get; set; }
    [Required]
    public required int ConsumptionTotalForCircle { get; set; }
    [Required]
    public required int TotalConsumptionAllFuels { get; set; }
}