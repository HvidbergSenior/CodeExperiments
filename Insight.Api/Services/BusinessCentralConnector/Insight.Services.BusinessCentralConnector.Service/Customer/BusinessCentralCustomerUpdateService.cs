using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Integration.Outbox;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using Insight.Services.BusinessCentralConnector.Service.Station;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static System.Formats.Asn1.AsnWriter;

namespace Insight.Services.BusinessCentralConnector.Service.Customer
{
    public class BusinessCentralCustomerUpdateService : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<BusinessCentralCustomerUpdateService> logger;
        private readonly IServiceScopeFactory factory;
        private bool IsEnabled { get; set; } = true;
        private bool firstRun = true;
        public BusinessCentralCustomerUpdateService(ILogger<BusinessCentralCustomerUpdateService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);

            while (
                !stoppingToken.IsCancellationRequested &&
                (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
            {
                firstRun = false;
                try
                {
                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();

                        var outbox = asyncScope.ServiceProvider.GetRequiredService<IOutbox>();
                        var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        await CreateOrUpdateCustomers(asyncScope, outbox, stoppingToken);

                        await CreateOrUpdateAllowedRawMaterials(asyncScope, outbox, stoppingToken);

                        await CreateOrUpdateCO2Targets(asyncScope, outbox, stoppingToken);

                        await CreateOrUpdateStations(asyncScope, outbox, stoppingToken);

                        await unitOfWork.Commit(stoppingToken);

                        logger.LogInformation("Executed {ServiceName}", nameof(BusinessCentralCustomerUpdateService));
                    }
                    else
                    {
                        logger.LogInformation("Skipped {ServiceName}", nameof(BusinessCentralCustomerUpdateService));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to execute {ServiceName}", nameof(BusinessCentralCustomerUpdateService));
                }
            }
        }

        private async Task CreateOrUpdateAllowedRawMaterials(AsyncServiceScope asyncScope, IOutbox outbox, CancellationToken stoppingToken)
        {
            var businessCentralRawMaterialService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralRawMaterialService>();
            var rawMaterials = await businessCentralRawMaterialService.GetAllAsync(stoppingToken);

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

                var rawMaterialFoundIntegrationEvent = AllowedRawMaterialUpdatedIntegrationEvent.Create(rawMaterialGroup.Key.CompanyId, rawMaterialGroup.Key.CustomerNumber, allowedRawMaterials);
                var outboxMessage = OutboxMessage.From(rawMaterialFoundIntegrationEvent);
                outbox.Add(outboxMessage);
            }
            logger.LogInformation("Updated {Count} allowed raw materials", rawMaterials.Count());
        }

        private async Task CreateOrUpdateCO2Targets(AsyncServiceScope asyncScope, IOutbox outbox, CancellationToken stoppingToken)
        {
            var businessCentralCo2TargetService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCo2TargetService>();

            var co2Targets = await businessCentralCo2TargetService.GetAllAsync(stoppingToken);
            foreach (var co2Target in co2Targets)
            {
                if (co2Target.CustomerNumber == null)
                    continue;
                var co2TargetIntegrationEvent = CreateCO2TargetIntegrationEvent(co2Target);
                var outboxMessage = OutboxMessage.From(co2TargetIntegrationEvent);
                outbox.Add(outboxMessage);               
            }
            logger.LogInformation("Updated {Count} CO2 targets", co2Targets.Count());
        }
        private CO2TargetUpdatedIntegrationEvent CreateCO2TargetIntegrationEvent(BusinessCentralCo2Target co2Target)
        {
            decimal co2Targetvalue = 0;
            if(co2Target.Co2Target > 0)
            {
                co2Targetvalue = co2Target.Co2Target / 100; // We store as fraction of 1
            }
            return CO2TargetUpdatedIntegrationEvent.Create(co2Target.CompanyId, co2Target.CustomerNumber!, co2Targetvalue);
        }

        private async Task CreateOrUpdateStations(AsyncServiceScope asyncScope, IOutbox outbox, CancellationToken stoppingToken)
        {
            var businessCentralStationService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralStationService>();
            var stations = await businessCentralStationService.GetAllAsync(stoppingToken);
            var ci = new CultureInfo("da-DK");
            foreach (var station in stations)
            {
                if (string.IsNullOrWhiteSpace(station.BFStationSystem))
                {
                    logger.LogWarning("Skipping station {StationName} due to missing station system", station.BFStationName);
                    continue;
                }
                var coordLat = station.BFStationLatitude;
                if (string.IsNullOrWhiteSpace(coordLat))
                {
                    coordLat = "0";
                }
                var coordLong = station.BFStationLongitude;
                if (string.IsNullOrWhiteSpace(coordLong))
                {
                    coordLong = "0";
                }
                var stationCreatedOrUpdatedIntegrationEvent = StationCreatedOrUpdatedIntegrationEvent.Create(station.CompanyId,
                                                                                                             station.Etag,
                                                                                                             station.BFStationName,
                                                                                                             station.StationNumber,
                                                                                                             station.BFStationSystem,
                                                                                                             station.BFStationAddress,
                                                                                                             station.BFStationAddress2,
                                                                                                             station.BFStationPostCode,
                                                                                                             station.BFStationCity,
                                                                                                             station.BFStationCountryRegionCode,
                                                                                                             double.Parse(coordLat, ci),
                                                                                                             double.Parse(coordLong, ci));

                var outboxMessage = OutboxMessage.From(stationCreatedOrUpdatedIntegrationEvent);
                outbox.Add(outboxMessage);
            }

            logger.LogInformation("Updated {Count} stations", stations.Count());
        }

        private async Task CreateOrUpdateCustomers(AsyncServiceScope asyncScope, IOutbox outbox, CancellationToken stoppingToken)
        {
            var businessCentralCustomerService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();
            var customers = await businessCentralCustomerService.GetAllAsync(stoppingToken);

            foreach (var customer in customers)
            {
                var customerCreatedOrUpdatedIntegrationEvent = CustomerCreatedOrUpdatedIntegrationEvent.Create(
                    customer.CompanyId,
                    customer.Etag,
                    customer.Number,
                    customer.CustomerNumber,
                    customer.CustomerName,
                    customer.CustomerAddress,
                    customer.CustomerCity,
                    customer.CustomerPostCode,
                    customer.CustomerCountryRegion,
                    customer.CustomerBillToNumber,
                    customer.CustomerDeliveryType,
                    customer.CustomerIndustry,
                    customer.CustomerBillToName,
                    customer.PaymentTermsCode,
                    customer.CreditLimit,
                    customer.CardCustomer,
                    customer.ShipmentMethodCode,
                    customer.ShippingAgentCode,
                    customer.OrganisationNumber,
                    customer.BalanceLCY,
                    customer.BalanceDueLCY,
                    customer.Blocked,
                    customer.OutstandingOrdersLCY,
                    customer.SalesPerson,
                    customer.PDIAndLDPointNumber,
                    customer.VATRegNumber);

                var outboxMessage = OutboxMessage.From(customerCreatedOrUpdatedIntegrationEvent);
                outbox.Add(outboxMessage);
            }
            logger.LogInformation("Updated {Count} customers", customers.Count());
        }
    }
}
