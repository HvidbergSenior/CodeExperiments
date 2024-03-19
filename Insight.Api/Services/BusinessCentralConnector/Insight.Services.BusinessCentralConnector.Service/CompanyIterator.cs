using Insight.BuildingBlocks.Exceptions;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using System.Net.Http.Json;
using Insight.Services.BusinessCentralConnector.Service.Company;

namespace Insight.Services.BusinessCentralConnector.Service
{
    public class CompanyIterator
    {
        private readonly HttpClient httpClient;
        private readonly BusinessCentralUrlHelper businessCentralUrlHelper;

        public CompanyIterator(HttpClient httpClient, BusinessCentralUrlHelper businessCentralUrlHelper)
        {
            this.httpClient = httpClient;
            this.businessCentralUrlHelper = businessCentralUrlHelper;
        }

        public async Task<BusinessCentralCompany[]> GetCompanies(bool isGlobalEndpoint,
            CancellationToken cancellationToken)
        {
            var response = await GetAllCompaniesFromDefaultEndpointAsync(cancellationToken);

            var companyResponse =
                await response.Content.ReadFromJsonAsync<BusinessCentralCompanyResponse>(
                    cancellationToken: cancellationToken);

            if (companyResponse == null)
            {
                throw new NotFoundException("Companies not returned from BC");
            }

            return isGlobalEndpoint ? companyResponse.Value.Take(1).ToArray() : companyResponse.Value;
        }

        private async Task<HttpResponseMessage> GetAllCompaniesFromDefaultEndpointAsync(
            CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(businessCentralUrlHelper.GetBusinessCentralCompaniesUrl(),
                cancellationToken);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}