namespace Insight.Services.BusinessCentralConnector.Service.Configuration
{
    public class BusinessCentralOptions : IBusinessCentralConfig
    {
        public const string DefaultConfigKey = "BusinessCentral";

        public string BaseUrl { get; set; } = "https://api.businesscentral.dynamics.com/v2.0";
        public string TenantId { get; set; } = "89efaadc-cfca-4ba1-994e-499e5ed6e241";
        public string EnvironmentPath { get; set; } = "Sandboxv20/api/BioFuel/Insight/v2.0";
        public string CompanyId { get; set; } = "ba61a6f1-3ef6-ed11-8848-6045bd937f02";
        public string ClientId { get; set; } = "56d0b3ac-2eb9-4f8c-9240-4554719573a2";
        public string ClientSecret { get; set; } = "JUb8Q~xZlNF4Kt90f~LwDRehPLHMwfGDZk7bibik";
        public string Scope { get; set; } = "https://api.businesscentral.dynamics.com/.default";
    }

    public interface IBusinessCentralConfig
    {
        public string BaseUrl { get; }
        public string TenantId { get; }
        public string EnvironmentPath { get; }
        public string CompanyId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Scope { get; }
    }
}
