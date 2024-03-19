using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders
{
    public class CO2TargetDefaultDataProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<CO2TargetDefaultDataProvider> logger;

        public CO2TargetDefaultDataProvider(IServiceScopeFactory serviceScopeFactory, ILogger<CO2TargetDefaultDataProvider> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var outbox = scope.ServiceProvider.GetRequiredService<IOutbox>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var businessCentralCo2TargetService = scope.ServiceProvider.GetRequiredService<BusinessCentralCo2TargetService>();

                var co2Targets = await businessCentralCo2TargetService.GetAllAsync(cancellation);
                foreach (var co2Target in co2Targets)
                {
                    if (co2Target.CustomerNumber == null)
                        continue;
                    var co2TargetIntegrationEvent = CreateCO2TargetIntegrationEvent(co2Target);
                    var outboxMessage = OutboxMessage.From(co2TargetIntegrationEvent);
                    outbox.Add(outboxMessage);
                }

                await unitOfWork.Commit(cancellation);
                logger.LogInformation("Added {Count} Co2 targets to outbox", co2Targets.Count());
            }
        }

        private CO2TargetUpdatedIntegrationEvent CreateCO2TargetIntegrationEvent(BusinessCentralCo2Target co2Target)
        {
            decimal co2Targetvalue = 0;
            if (co2Target.Co2Target > 0)
            {
                co2Targetvalue = co2Target.Co2Target / 100; // We store as fraction of 1
            }
            return CO2TargetUpdatedIntegrationEvent.Create(co2Target.CompanyId, co2Target.CustomerNumber!, co2Targetvalue);
        }
    }
}
