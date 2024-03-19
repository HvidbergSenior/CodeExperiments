using Insight.BuildingBlocks.Integration;

namespace Insight.Services.BusinessCentralConnector.Integration
{
    public class AllowedRawMaterialUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid CompanyId { get; private set; }
        public string CustomerNumber { get; private set; }
        public Dictionary<string, bool> AllowedRawMaterials { get; private set; }

        private AllowedRawMaterialUpdatedIntegrationEvent() : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId = Guid.Empty;
            CustomerNumber = string.Empty;
            AllowedRawMaterials = new Dictionary<string, bool>();
        }

        private AllowedRawMaterialUpdatedIntegrationEvent(Guid companyId, string customerNumber, Dictionary<string,bool> allowedRawMaterials) : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId = companyId;
            CustomerNumber = customerNumber;
            AllowedRawMaterials = allowedRawMaterials;
        }

        public static AllowedRawMaterialUpdatedIntegrationEvent Create(Guid companyId, string customerNumber, Dictionary<string, bool> allowedRawMaterials)
        {
            return new AllowedRawMaterialUpdatedIntegrationEvent(companyId, customerNumber, allowedRawMaterials);
        }
    }
}