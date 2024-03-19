using Insight.Customers.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Insight.FuelTransactions.Domain;
using FluentAssertions;
using Insight.WebApplication.Services;
using Insight.BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Insight.Tests.End2End.Services
{
    [Collection("End2End")]
    public class CustomerUpdateServiceTests
    {

        private readonly WebAppFixture webAppFixture;

        public CustomerUpdateServiceTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
        }

        [Fact]
        public async Task MakeSureFuelTransactionsAreUpdatedWithCustomerId()
        {
            var fuelRepo = webAppFixture.AppFactory.Services.GetRequiredService<IFuelTransactionsRepository>();
            var customerRepo = webAppFixture.AppFactory.Services.GetRequiredService<ICustomerRepository>();
            var uow = webAppFixture.AppFactory.Services.GetRequiredService<IUnitOfWork>();
            var customer = Any.Customer();
            await customerRepo.Add(customer);
            var fuelTransaction = Any.FuelTransaction(customer.CustomerDetails.CustomerNumber.Value);

            await fuelRepo.Add(fuelTransaction);
            await fuelRepo.SaveChanges();
            await customerRepo.SaveChanges();
            await uow.Commit(CancellationToken.None);

            var serviceScope = webAppFixture.AppFactory.Services.GetRequiredService<IServiceScopeFactory>();
            var logger = webAppFixture.AppFactory.Services.GetRequiredService<ILogger<EntityCustomerRelationUpdateService>>();
            var waitSpan = TimeSpan.FromSeconds(1);
            var customerUpdaterService = new EntityCustomerRelationUpdateService(serviceScope, logger) { Period = waitSpan };
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            await customerUpdaterService.StartAsync(token);

            while (customerUpdaterService.IterationCount < 1)
            {
                await Task.Delay(1000);
            }
            cancellationTokenSource.Cancel();
            uow.EjectAllOfType<FuelTransaction>();
            var updatedFuelTransaction = await fuelRepo.GetById(fuelTransaction.Id);

            updatedFuelTransaction.CustomerId.Value.Should().Be(customer.CustomerId.Value);
        }
    }
}
