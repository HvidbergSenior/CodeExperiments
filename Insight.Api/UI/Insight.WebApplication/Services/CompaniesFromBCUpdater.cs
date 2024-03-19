using Insight.BuildingBlocks.Infrastructure;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Helpers;

namespace Insight.WebApplication.Services
{
    public class CompaniesFromBCUpdater : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<CompaniesFromBCUpdater> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;

        public CompaniesFromBCUpdater(ILogger<CompaniesFromBCUpdater> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);

            while (
                !stoppingToken.IsCancellationRequested &&
                (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
            {
                firstRun = false;
                await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();

                var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var companyRepository = asyncScope.ServiceProvider.GetRequiredService<ICompanyRepository>();
                var httpClientFactory = asyncScope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
                var businessCentralAuthHelper = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralAuthHelper>();
                var businessCentralUrlHelper = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralUrlHelper>();
                var accessToken = await businessCentralAuthHelper.GetAccessTokenAsync();

                using (var client = httpClientFactory.CreateClient("BCClient"))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                    var companyIterator = new CompanyIterator(client, businessCentralUrlHelper);
                   
                    var companies = await companyIterator.GetCompanies(false, stoppingToken);

                    foreach (var bcCompany in companies)
                    {
                        var companyFromDB = await companyRepository.GetCompanyByCompanyId(CompanyId.Create(bcCompany.Id), stoppingToken);
                        if (companyFromDB == null)
                        {
                            logger.LogInformation("Adding company {Company}", bcCompany.Name);
                            await companyRepository.Add(bcCompany.ToCompany(), stoppingToken);
                        }
                        else
                        {
                            if (companyFromDB.SourcesystemEtag.Value == bcCompany.SystemVersion)
                            {
                                continue;
                            }
                            logger.LogInformation("Updated company {Company}", bcCompany.Name);
                            var tempCompany = bcCompany.ToCompany();

                            companyFromDB.Update(tempCompany.CompanyName, tempCompany.SourcesystemEtag);
                            await companyRepository.Update(companyFromDB, stoppingToken);
                        }
                    }
                    await companyRepository.SaveChanges(stoppingToken);
                    await unitOfWork.Commit(stoppingToken);
                }
            }
        }
    }
}
