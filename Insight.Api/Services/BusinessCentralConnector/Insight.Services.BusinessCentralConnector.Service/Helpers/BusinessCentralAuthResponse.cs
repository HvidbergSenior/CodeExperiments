namespace Insight.Services.BusinessCentralConnector.Service.Helpers
{
    public class BusinessCentralAuthResponse
    {
        public required string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public required string access_token { get; set; }
    }
}
