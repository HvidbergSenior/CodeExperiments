namespace Insight.Services.BusinessCentralConnector.Service.Company
{
    public class BusinessCentralCompany : BusinessCentralEntity
    { 
        public Guid Id { get; set; } = Guid.Empty;
        public string SystemVersion { get; set; } = string.Empty;
        public int Timestamp { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string BusinessProfileId { get; set; } = string.Empty;
        public DateTimeOffset SystemCreatedAt { get; set; }
        public Guid SystemCreatedBy { get; set; } = Guid.Empty;
        public DateTimeOffset SystemModifiedAt { get; set; }
        public Guid SystemModifiedBy { get; set; } = Guid.Empty;
    }
}
