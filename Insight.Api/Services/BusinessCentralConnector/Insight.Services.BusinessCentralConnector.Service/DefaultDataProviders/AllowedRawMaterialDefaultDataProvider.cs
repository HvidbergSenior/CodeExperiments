using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Insight.Services.BusinessCentralConnector.Service.DefaultDataProviders
{
    public class AllowedRawMaterialDefaultDataProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<AllowedRawMaterialDefaultDataProvider> logger;

        public AllowedRawMaterialDefaultDataProvider(IServiceScopeFactory serviceScopeFactory, ILogger<AllowedRawMaterialDefaultDataProvider> logger)
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

                var businessCentralRawMaterialService = scope.ServiceProvider.GetRequiredService<BusinessCentralRawMaterialService>();

                var rawMaterials = await businessCentralRawMaterialService.GetAllAsync(cancellation);
                foreach (var rawMaterialGroup in rawMaterials.GroupBy(c => new { c.CompanyId, c.CustomerNumber }))
                {
                    var allowedRawMaterials = new Dictionary<string, bool>();

                    foreach (var rawMaterial in rawMaterialGroup)
                    {
                        if (string.IsNullOrWhiteSpace(rawMaterial.Feedstock) || string.IsNullOrWhiteSpace(rawMaterial.IncludeExclude))
                        {
                            logger.LogWarning("Raw material for customer {Customer} in Company {CompanyId} has no feedstock or include/exclude", rawMaterial.CustomerNumber, rawMaterial.CompanyId);
                            continue;
                        }
                        allowedRawMaterials.Add(rawMaterial.Feedstock, rawMaterial.IncludeExclude == "Include");
                    }

                    var rawMaterialFoundIntegrationEvent = CreateRawMaterialIntegrationEvent(rawMaterialGroup.Key.CompanyId, rawMaterialGroup.Key.CustomerNumber, allowedRawMaterials);
                    var outboxMessage = OutboxMessage.From(rawMaterialFoundIntegrationEvent);
                    outbox.Add(outboxMessage);
                }


                await unitOfWork.Commit(cancellation);
                logger.LogInformation("Added {Count} raw materials to outbox", rawMaterials.Count());
            }
        }

        private static AllowedRawMaterialUpdatedIntegrationEvent CreateRawMaterialIntegrationEvent(Guid companyId, string customerNumber, Dictionary<string, bool> allowedRawMaterials)
        {
            return AllowedRawMaterialUpdatedIntegrationEvent.Create(companyId, customerNumber, allowedRawMaterials);
        }
    }
}