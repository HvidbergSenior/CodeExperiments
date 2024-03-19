using Insight.BuildingBlocks.Integration;

namespace Insight.Services.BusinessCentralConnector.Integration
{
    public class CO2TargetUpdatedIntegrationEvent : IntegrationEvent
    {
        public string CustomerNumber { get; private set; }
        public Guid CompanyId { get; private set; }
        public decimal CO2Target { get; private set; }


        private CO2TargetUpdatedIntegrationEvent() : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId= Guid.NewGuid();
            CustomerNumber = string.Empty;
        }

        private CO2TargetUpdatedIntegrationEvent(Guid companyId, string customerNumber, decimal co2Target) : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId= companyId;
            CustomerNumber = customerNumber;
            CO2Target= co2Target;
        }

        public static CO2TargetUpdatedIntegrationEvent Create(Guid companyId, string customerNumber, decimal co2Target)
        {
            return new CO2TargetUpdatedIntegrationEvent(companyId, customerNumber, co2Target);
        }
    }
}
