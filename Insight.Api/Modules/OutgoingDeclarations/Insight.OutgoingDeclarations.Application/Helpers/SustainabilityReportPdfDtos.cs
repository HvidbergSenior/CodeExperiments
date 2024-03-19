using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.Helpers;



public sealed class Emissionsstats
{
    [Required]
    public required int AchievedEmissionReductions { get; set; }
    [Required]
    public required int NetEmission { get; set; }
    [Required]
    public required decimal EmissionSavingsForCircle { get; set; }
}

public sealed class Consumptionstatscontinued
{
    [Required]
    public int CurrentConsumptions { get; set; }
    [Required]
    public int PrevConsumptions { get; set; }
    [Required]
    public int RenewCurrentConsumptions { get; set; }
    [Required]
    public int PrevRenewConsumptions { get; set; }
    [Required]
    public int ConsumptionPercentage { get; set; }
}



public sealed class NameValuePair
{
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Value { get; set; }

    public NameValuePair(string name, decimal value)
    {
        Name = name;
        Value = value;
    }
}


public sealed class Series
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required IReadOnlyList<int> Data { get; set; }

    public Series()
    {
    }
}

public sealed class Progress
{
    [Required]
    public required IReadOnlyList<decimal> Emissions { get; set; }
    [Required]
    public required IReadOnlyList<decimal> EmissionReduction { get; set; }
    [Required]
    public required IReadOnlyList<string> Categories { get; set; }
}

public sealed class Declarationinfo
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public DateTime DateOfIssuance { get; set; }
}

public sealed class Recipient
{
    [Required]
    public required Address Address { get; set; }
    [Required]
    public DateTime PeriodFrom { get; set; }
    [Required]
    public DateTime PeriodTo { get; set; }
}

public sealed class Address
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Street { get; set; }
    [Required]
    public required string StreetNumber { get; set; }
    [Required]
    public required string ZipCode { get; set; }
    [Required]
    public required string City { get; set; }
    [Required]
    public required string Country { get; set; }
}

public sealed class Renewablefuelsupplier
{
    [Required]
    public required Address Address { get; set; }
    [Required]
    public required string CertificateSystem { get; set; }
    [Required]
    public required string CertificateNumber { get; set; }
}

public sealed class Renewablefuel
{
    [Required]
    public int Volume { get; set; }
    [Required]
    public required string Product { get; set; }
    [Required]
    public int EnergyContent { get; set; }
}

public sealed class Scopeofcertificationandghgemission
{
    [Required]
    public required bool EuRedCompliantMaterial { get; set; }
    [Required]
    public required bool IsccCompliantMaterial { get; set; }
    [Required]
    public required string ChainOfCustodyOption { get; set; }
    [Required]
    public required bool TotalDefaultValueAccordingToRed2Applied { get; set; }
}

public sealed class Rawmaterialsustainability
{
    [Required]
    public required string RawMaterial { get; set; }
    [Required]
    public required string CountryOfOrigin { get; set; }
    [Required]
    public required string ProductionCountry { get; set; }
    [Required]
    public required string DateOfInstallation { get; set; }
}

public sealed class Scopeofcertificationofrawmaterial
{
    [Required]
    public required bool Option1 { get; set; }
    [Required]
    public required bool Option2 { get; set; }
    [Required]
    public required bool Option3 { get; set; }
    [Required]
    public required bool Option4 { get; set; }
    [Required]
    public required string Option5 { get; set; }
}

public sealed class Lifecyclegreenhousegasemissions
{
    [Required]
    public decimal ExtractionOrCultivation { get; set; }
    [Required]
    public int LandUse { get; set; }
    [Required]
    public decimal Processing { get; set; }
    [Required]
    public decimal TransportAndDistribution { get; set; }
    [Required]
    public int FuelInUse { get; set; }
    [Required]
    public int SoilCarbonAccumulation { get; set; }
    [Required]
    public int CarbonCaptureAndGeologicalStorage { get; set; }
    [Required]
    public int CarbonCaptureAndReplacement { get; set; }
    [Required]
    public decimal TotalGHGEmissionFromSupplyAndUseOfFuel { get; set; }
}

public sealed class Greenhousegasemissionssavings
{
    [Required]
    public decimal GhgPercent { get; set; }
}

public sealed class Feedstock
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public decimal Percentage { get; set; }
}

public sealed class Countryoforigin
{
    [Required]
    public required string CooFeedstock { get; set; }
    [Required]
    public decimal Volume { get; set; }
    [Required]
    public decimal AverageSavings { get; set; }
}
public sealed class Country
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public decimal Percentage { get; set; }
}
public sealed class ProductSpecificationItem
{
    [Required]
    public required string FuelType { get; set; }
    [Required]
    public decimal Volume { get; set; }
    [Required]
    public decimal GhgBaseline { get; set; } 
    [Required]
    public decimal GhgEmissionSaving { get; set; } 
    [Required]
    public decimal AchievedEmissionReduction { get; set; }
    [Required]
    public decimal NetEmission { get; set; }
}
public sealed class PdfReportPosResponse
{
    [Required]
    public required Recipient Recipient { get; set; }
    [Required]
    public required Declarationinfo Declarationinfo { get; set; }
    [Required]
    public required Renewablefuelsupplier Renewablefuelsupplier { get; set; }
    [Required]
    public required Renewablefuel Renewablefuel { get; set; }
    [Required]
    public required Scopeofcertificationandghgemission Scopeofcertificationandghgemission { get; set; }
    [Required]
    public required Rawmaterialsustainability Rawmaterialsustainability { get; set; }
    [Required]
    public required Scopeofcertificationofrawmaterial Scopeofcertificationofrawmaterial { get; set; }
    [Required]
    public required Lifecyclegreenhousegasemissions Lifecyclegreenhousegasemissions { get; set; }
    [Required]
    public required Greenhousegasemissionssavings Greenhousegasemissionssavings { get; set; }

}
