using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.FuelTransactions.Domain;
using Marten;
using Marten.PLv8.Patching;
using Weasel.Postgresql.SqlGeneration;

namespace Insight.WebApplication.Services
{
    public class EntityCustomerRelationUpdateService : BackgroundService
    {
        public TimeSpan Period { get; set; } = TimeSpan.FromHours(1);
        private readonly IServiceScopeFactory factory;
        private readonly ILogger<EntityCustomerRelationUpdateService> logger;
        public int IterationCount { get; private set; }
        private bool firstRun = true;

        public EntityCustomerRelationUpdateService(IServiceScopeFactory factory, ILogger<EntityCustomerRelationUpdateService> logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(Period);
            while (
                !stoppingToken.IsCancellationRequested && (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
            {
                firstRun = false;
                using var asyncScope = factory.CreateAsyncScope();

                var customerRepo = asyncScope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                var fuelRepo = asyncScope.ServiceProvider.GetRequiredService<IFuelTransactionsRepository>();

                bool moreData = true;
                var pageNumber = 1;
                var pagesize = 50;
                while(moreData)
                {
                    var customers = await customerRepo.GetCustomersByPageNumberAndPageSizeAsync(pageNumber, pagesize, stoppingToken);
                    if(customers.Count() < pagesize)
                    {
                        moreData = false;
                    }
                    pageNumber++;
                    var documentSession = asyncScope.ServiceProvider.GetRequiredService<IDocumentSession>();

                    foreach (var customer in customers)
                    {
                        try
                        {
                            var whereFragmentSql = $"data ->'CustomerId'->>'Value' = '{Guid.Empty}' AND data->'CustomerNumber'->>'Value' = '{customer.CustomerDetails.CustomerNumber.Value}'";

                            var customerId = FuelTransactionCustomerId.Create(customer.CustomerId.Value);
                            var customerName = CustomerName.Create(customer.CustomerDetails.CustomerName.Value);
                            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.CustomerId), customerId);
                            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.CustomerName), customerName);
                            await documentSession.SaveChangesAsync(stoppingToken);
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, "Error updating entity with customer id");
                            throw;
                        }

                        try
                        {
                            var whereFragmentSql = $"data ->'AccountCustomerId'->>'Value' = '{Guid.Empty}' AND data->'AccountNumber'->>'Value' = '{customer.CustomerDetails.CustomerNumber.Value}'";

                            var customerId = FuelTransactionCustomerId.Create(customer.CustomerId.Value);                            
                            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.AccountCustomerId), customerId);                            
                            await documentSession.SaveChangesAsync(stoppingToken);
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, "Error updating entity with customer id");
                            throw;
                        }
                    }
                    logger.LogInformation("Executed {ServiceName}", nameof(EntityCustomerRelationUpdateService));
                    IterationCount++;
                }
            }
        }
    }
}
