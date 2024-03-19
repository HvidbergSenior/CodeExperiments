namespace Insight.BuildingBlocks.Telemetry;

public class ApplicationInsightsConfig : IApplicationInsightsConfig
{
    public const string DefaultConfigKey = "ApplicationInsights";
    public string ConnectionString { get; set; } = string.Empty;
    
}

public interface IApplicationInsightsConfig
{
    string ConnectionString { get; }
}