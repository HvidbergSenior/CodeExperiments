using System.Data;
using System.Net.Http.Json;
using System.Text;
using Insight.Services.BusinessCentralConnector.Service.Configuration;
using Microsoft.Extensions.Logging;

namespace Insight.Services.BusinessCentralConnector.Service.Helpers
{
    public class BusinessCentralAuthHelper
    {
        private readonly IBusinessCentralConfig _businessCentralConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BusinessCentralAuthHelper> _logger;

        public BusinessCentralAuthHelper(IBusinessCentralConfig businessCentralConfig, IHttpClientFactory httpClientFactory, ILogger<BusinessCentralAuthHelper> logger)
        {
            _businessCentralConfig = businessCentralConfig;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient("BCClient"))
                {
                    var url = $"https://login.microsoftonline.com/{_businessCentralConfig.TenantId}/oauth2/v2.0/token";

                    var formData = new
                    {
                        grant_type = "client_credentials",
                        client_id = _businessCentralConfig.ClientId,
                        client_secret = _businessCentralConfig.ClientSecret,
                        scope = _businessCentralConfig.Scope
                    };

                    var urlEncodedData = ToUrlEncodedString(formData);

                    var content = new StringContent(urlEncodedData, Encoding.UTF8, "application/x-www-form-urlencoded");

                    var response = await client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();

                    var tokenResponse = await response.Content.ReadFromJsonAsync<BusinessCentralAuthResponse>();
                    if (tokenResponse == null)
                    {
                        throw new ArgumentNullException(nameof(tokenResponse));
                    }
                    return tokenResponse.access_token;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    $"Error when trying to receive Access Token from Business Central. Error message: {{@errorMessage}}",
                    ex.Message);
                throw;
            }
        }

        static string ToUrlEncodedString(object values)
        {
            var properties = values.GetType().GetProperties();
            var keyValuePairs = properties.Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(values)?.ToString() ?? "")}");
            return string.Join("&", keyValuePairs);
        }
    }
}