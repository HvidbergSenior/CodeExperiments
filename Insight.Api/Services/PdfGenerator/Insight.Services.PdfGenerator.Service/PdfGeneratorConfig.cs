namespace Insight.Services.PdfGenerator.Service
{
    public class PdfGeneratorConfig : IPdfGeneratorConfig
    {
        public const string DefaultConfigKey = "PdfGenerator";

        public string BrowserWsEndpoint { get; set; } = "ws://localhost:3100/";
        public int Timeout { get; set; } = 7000;

        public bool IgnoreHttpsErrors { get; set; }
    }
}

public interface IPdfGeneratorConfig
{
    public string BrowserWsEndpoint { get; }
    public int Timeout { get;}
    
    public bool IgnoreHttpsErrors { get; }
}
