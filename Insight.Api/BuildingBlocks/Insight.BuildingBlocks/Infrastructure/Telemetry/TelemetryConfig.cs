namespace Insight.BuildingBlocks.Telemetry
{
    public class TelemetryConfig : ITelemetryConfig
    {
        public const string DefaultConfigKey = "Telemetry";
        public const string DefaultServiceName = "insight-web-app";

        public string ServiceName { get; } = DefaultServiceName;
    }
}

public interface ITelemetryConfig
{
    string ServiceName { get; }
}