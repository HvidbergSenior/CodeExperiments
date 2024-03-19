using Microsoft.Extensions.Logging;

namespace Insight.Services.BusinessCentralConnector.Service.Station
{
    public class BusinessCentralStationService : BusinessCentralService<BusinessCentralStation>
    {
        private const string ENTITY_NAME = "insight_Station";
        private const int PAGE_SIZE = 500;
        private readonly IBusinessCentralApiClient businessCentralApiClient;
        private readonly ILogger<BusinessCentralStationService> logger;

        public BusinessCentralStationService(IBusinessCentralApiClient businessCentralApiClient, ILogger<BusinessCentralStationService> logger) : base(businessCentralApiClient)
        {
            this.businessCentralApiClient = businessCentralApiClient;
            this.logger = logger;
        }

        public override string GetEntityName()
        {
            return ENTITY_NAME;
        }

        public override int GetPageSize()
        {
            return PAGE_SIZE;
        }
        public override bool IsGlobalEndpoint()
        {
            return true;
        }

        public async Task<BusinessCentralStation?> GetStationByStationNumber(string stationNumber, CancellationToken cancellationToken)
        {
            var customQuery = $"$filter=StationNumber eq '{stationNumber}'";

            var stations = await businessCentralApiClient.GetItemsByCustomQueryAsync<BusinessCentralStation>(GetEntityName(), GetPageSize(), customQuery, cancellationToken, true);
            if(stations.Count() > 1)
            {
                logger.LogWarning("More than one station found with station number {StationNumber}", stationNumber);
                //throw new PlatformNotSupportedException($"More than one station found with station number {stationNumber}");
            }
            return stations.FirstOrDefault();
        }
    }
}
