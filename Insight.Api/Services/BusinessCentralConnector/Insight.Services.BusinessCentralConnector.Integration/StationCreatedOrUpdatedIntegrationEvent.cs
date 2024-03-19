using Insight.BuildingBlocks.Integration;

namespace Insight.Services.BusinessCentralConnector.Integration
{
    public class StationCreatedOrUpdatedIntegrationEvent : IntegrationEvent
    {
        public Guid CompanyId { get; set; } = Guid.Empty;
        public string SourceETag { get; set; } = string.Empty;
        public string StationNumber { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string StationSystem { get; set; } = string.Empty;
        public string StationAddress { get; set; } = string.Empty;
        public string StationCity { get; set; } = string.Empty;
        public string StationCountry { get; set; } = string.Empty;
        public string StationPostalCode { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private StationCreatedOrUpdatedIntegrationEvent() : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            
        }

        private StationCreatedOrUpdatedIntegrationEvent(Guid companyId,
                                                        string sourceETag,
                                                        string stationName,
                                                        string stationNumber,
                                                        string stationSystem,
                                                        string stationAddress,
                                                        string stationCity,
                                                        string stationCountry,
                                                        string stationPostalCode,
                                                        string address2,
                                                        double latitude,
                                                        double longitude) : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            CompanyId = companyId;
            SourceETag = sourceETag;            
            StationNumber = stationNumber;
            StationName = stationName;
            StationSystem = stationSystem;
            StationAddress = stationAddress;
            StationCity = stationCity;
            StationCountry = stationCountry;
            StationPostalCode = stationPostalCode;
            Address2 = address2;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static StationCreatedOrUpdatedIntegrationEvent Create(Guid companyId,
                                                                     string sourceETag,
                                                                     string stationName,
                                                                     string stationNumber,
                                                                     string stationSystem,
                                                                     string stationAddress,
                                                                     string stationCity,
                                                                     string stationCountry,
                                                                     string stationPostalCode,
                                                                     string address2,
                                                                     double latitude,
                                                                     double longitude)
        {
            return new StationCreatedOrUpdatedIntegrationEvent(companyId,
                                                               sourceETag,
                                                               stationName,
                                                               stationNumber,
                                                               stationSystem,
                                                               stationAddress,
                                                               stationCity,
                                                               stationCountry,
                                                               stationPostalCode,
                                                               address2,
                                                               latitude,
                                                               longitude);
        }

    }
}
