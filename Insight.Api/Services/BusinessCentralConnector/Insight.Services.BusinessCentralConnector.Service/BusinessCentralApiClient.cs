using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Newtonsoft.Json;
using Serilog.Core;
using System.Net.Http.Json;
using Insight.Services.BusinessCentralConnector.Service.Company;

namespace Insight.Services.BusinessCentralConnector.Service
{
    public class BusinessCentralApiClient : IBusinessCentralApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly BusinessCentralAuthHelper businessCentralAuthHelper;
        private readonly BusinessCentralUrlHelper businessCentralUrlHelper;

        public BusinessCentralApiClient(IHttpClientFactory httpClientFactory, BusinessCentralAuthHelper businessCentralAuthHelper, BusinessCentralUrlHelper businessCentralUrlHelper)
        {
            this.httpClientFactory = httpClientFactory;
            this.businessCentralAuthHelper = businessCentralAuthHelper;
            this.businessCentralUrlHelper = businessCentralUrlHelper;
        }

        public async Task<IEnumerable<T>> GetAllTransactionsAfterDateAsync<T>(DateTimeOffset fromDate, string entityName, int pageSize,
            CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity
        {
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companiesToIterate = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);

                foreach (var company in companiesToIterate)
                {
                    var amountOfEntitiesToSkip = 0;
                    var moreDataExists = true;

                    while (moreDataExists)
                    {
                        var url = businessCentralUrlHelper.GetBusinessCentralTransactionUrlWithPaginationAndFromDate(fromDate, company.Id, entityName, pageSize, amountOfEntitiesToSkip);

                        var response = await client.GetAsync(url, cancellationToken);
                        response.EnsureSuccessStatusCode();

                        var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);

                        if (entities is not null && entities.Value.Length > 0)
                        {
                            foreach (var entity in entities.Value)
                            {
                                entity.SetCompanyId(company.Id);
                                entity.SetCompanyName(company.Name);
                            }

                            entityValues.AddRange(entities.Value);
                            amountOfEntitiesToSkip += pageSize;
                        }
                        else
                        {
                            moreDataExists = false;
                        }
                    }
                }
                return entityValues;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string entityName, int pageSize, CancellationToken cancellationToken, bool isGlobalEndpoint = false) where T : BusinessCentralEntity
        {
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companiesToIterate = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);

                foreach (var company in companiesToIterate)
                {
                    var amountOfEntitiesToSkip = 0;
                    var moreDataExists = true;

                    while (moreDataExists)
                    {
                        var url = businessCentralUrlHelper.GetBusinessCentralUrlWithPagination(company.Id, entityName, pageSize, amountOfEntitiesToSkip);

                        var response = await client.GetAsync(url, cancellationToken);
                        response.EnsureSuccessStatusCode();

                        var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);

                        if (entities is not null && entities.Value.Length > 0)
                        {
                            foreach (var entity in entities.Value)
                            {
                                entity.SetCompanyId(company.Id);
                                entity.SetCompanyName(company.Name);
                            }

                            entityValues.AddRange(entities.Value);
                            amountOfEntitiesToSkip += pageSize;
                        }
                        else
                        {
                            moreDataExists = false;
                        }
                    }
                }
                return entityValues;
            }
        }

        public async Task<IEnumerable<T>> GetTransactionsAfterDateByPageSizeAsync<T>(DateTimeOffset fromDate, int pageSize, string entityName, int transactionsToSkip,
            CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity
        {
            //Todo: Where to handle that we only use this method with Transactions?
            //Maybe check on entityName? equals insight_Transactions....
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companiesToIterate = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);

                foreach (var company in companiesToIterate)
                {
                    var url = businessCentralUrlHelper.GetBusinessCentralTransactionUrlWithPaginationAndFromDate(fromDate, company.Id, entityName, pageSize, transactionsToSkip);

                    var response = await client.GetAsync(url, cancellationToken);
                    var responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);

                    if (entities is not null && entities.Value.Length > 0)
                    {
                        foreach (var entity in entities.Value)
                        {
                            entity.SetCompanyId(company.Id);
                            entity.SetCompanyName(company.Name);
                        }

                        entityValues.AddRange(entities.Value);
                    }
                }
                return entityValues;
            }
        }

        public async Task<IEnumerable<T>> GetItemsByCustomQueryAsync<T>(string entityName, int pageSize, string oDataQuery, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity
        {
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companiesToIterate = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);

                foreach (var company in companiesToIterate)
                {
                    var amountOfEntitiesToSkip = 0;
                    var moreDataExists = true;

                    while (moreDataExists)
                    {
                        var url = businessCentralUrlHelper.GetBusinessCentralUrlWithPagination(company.Id, entityName, pageSize, amountOfEntitiesToSkip);

                        url = $"{url}&{oDataQuery}";

                        var response = await client.GetAsync(url, cancellationToken);
                        response.EnsureSuccessStatusCode();
                        try
                        {
                            var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);
                            if (entities is not null && entities.Value.Length <= pageSize)
                            {
                                moreDataExists = false;
                            }

                            if (entities is not null && entities.Value.Length > 0)
                            {
                                foreach (var entity in entities.Value)
                                {
                                    entity.SetCompanyId(company.Id);
                                    entity.SetCompanyName(company.Name);
                                }

                                entityValues.AddRange(entities.Value);
                                amountOfEntitiesToSkip += pageSize;
                            }
                            else
                            {
                                moreDataExists = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var deserialized = JsonConvert.DeserializeObject<BusinessCentralResponse<T>>(content);
                            var lol = ex.Message;
                            throw;
                        }
                    }
                }
                return entityValues;
            }
        }

        public async Task<BusinessCentralCompany[]> GetCompaniesAsync<T>(bool isGlobalEndpoint, CancellationToken cancellationToken)
        {
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companies = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);
                return companies;
            }
        }

        public async Task<IEnumerable<T>> GetItemsByCustomQueryAndCompanyAsync<T>(string entityName, int pageSize, string oDataQuery, Guid companyId, string companyName, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity
        {
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var amountOfEntitiesToSkip = 0;
                var moreDataExists = true;

                while (moreDataExists)
                {
                    var url = businessCentralUrlHelper.GetBusinessCentralUrlWithPagination(companyId, entityName, pageSize, amountOfEntitiesToSkip);

                    url = $"{url}&{oDataQuery}";

                    var response = await client.GetAsync(url, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    try
                    {
                        var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);
                        if (entities is not null && entities.Value.Length <= pageSize)
                        {
                            moreDataExists = false;
                        }

                        if (entities is not null && entities.Value.Length > 0)
                        {
                            foreach (var entity in entities.Value)
                            {
                                entity.SetCompanyId(companyId);
                                entity.SetCompanyName(companyName);
                            }

                            entityValues.AddRange(entities.Value);
                            amountOfEntitiesToSkip += pageSize;
                        }
                        else
                        {
                            moreDataExists = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var deserialized = JsonConvert.DeserializeObject<BusinessCentralResponse<T>>(content);
                        var lol = ex.Message;
                        throw;
                    }

                }
                return entityValues;
            }
        }

        public async Task<IEnumerable<T>> GetTransactionsByPageSizeAsync<T>(int pageSize, string entityName, int transactionsToSkip, CancellationToken cancellationToken, bool isGlobalEndpoint) where T : BusinessCentralEntity
        {
            //Todo: Where to handle that we only use this method with Transactions?
            //Maybe check on entityName? equals insight_Transactions....
            var entityValues = new List<T>();
            var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

            using (var client = httpClientFactory.CreateClient("BCClient"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);

                var companiesToIterate = await companyIterator.GetCompanies(isGlobalEndpoint, cancellationToken);

                foreach (var company in companiesToIterate)
                {
                    var url = businessCentralUrlHelper.GetBusinessCentralUrlWithPagination(company.Id, entityName, pageSize, transactionsToSkip);

                    var response = await client.GetAsync(url, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var entities = await response.Content.ReadFromJsonAsync<BusinessCentralResponse<T>>(cancellationToken: cancellationToken);

                    if (entities is not null && entities.Value.Length > 0)
                    {
                        foreach (var entity in entities.Value)
                        {
                            entity.SetCompanyId(company.Id);
                            entity.SetCompanyName(company.Name);
                        }

                        entityValues.AddRange(entities.Value);
                    }
                }
                return entityValues;
            }
        }
    }
}
