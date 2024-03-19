using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class Station : Entity
    {
        public StationId StationId { get; private set; } = StationId.Empty();
        public SourceETag SourceETag { get; private set; } = SourceETag.Empty();
        public StationNumber StationNumber { get; private set; } = StationNumber.Empty();
        public StationName StationName { get; private set; } = StationName.Empty();
        public StationAddress StationAddress { get; private set; } = StationAddress.Empty();
        public StationAddress StationAddress2 { get; private set; } = StationAddress.Empty();
        public StationPostCode StationPostCode { get; private set; } = StationPostCode.Empty();
        public StationCity StationCity { get; private set; } = StationCity.Empty();
        public StationCountryRegionCode StationCountryRegionCode { get; private set; } = StationCountryRegionCode.Empty();
        public StationLatitude StationLatitude { get; private set; } = StationLatitude.Empty();
        public StationLongitude StationLongitude { get; private set; } = StationLongitude.Empty();
        public StationSystem StationSystem { get; private set; }

        private Station()
        {
            // For serialization
        }

        private Station(StationId stationId,
                        SourceETag sourceETag,
                        StationNumber stationNumber,
                        StationName stationName,
                        StationAddress stationAddress,
                        StationAddress stationAddress2,
                        StationPostCode stationPostCode,
                        StationCity stationCity,
                        StationCountryRegionCode stationCountryRegionCode,
                        StationLatitude stationLatitude,
                        StationLongitude stationLongitude,
                        StationSystem stationSystem)
        {
            StationId = stationId;
            Id = StationId.Value;
            SourceETag = sourceETag;
            StationNumber = stationNumber;
            StationName = stationName;
            StationAddress = stationAddress;
            StationAddress2 = stationAddress2;
            StationPostCode = stationPostCode;
            StationCity = stationCity;
            StationCountryRegionCode = stationCountryRegionCode;
            StationLatitude = stationLatitude;
            StationLongitude = stationLongitude;
            StationSystem = stationSystem;
        }

        public void Update(StationName stationName,
                           StationAddress stationAddress,
                           StationAddress stationAddress2,
                           StationPostCode stationPostCode,
                           StationCity stationCity,
                           StationCountryRegionCode stationCountryRegionCode,
                           StationLatitude stationLatitude,
                           StationLongitude stationLongitude,
                           StationSystem stationSystem)
        {           
            StationName = stationName;
            StationAddress = stationAddress;
            StationAddress2 = stationAddress2;
            StationPostCode = stationPostCode;
            StationCity = stationCity;
            StationCountryRegionCode = stationCountryRegionCode;
            StationLatitude = stationLatitude;
            StationLongitude = stationLongitude;
            StationSystem = stationSystem;
        }


        public static Station Create(StationId stationId,
                                     SourceETag sourceETag,
                                     StationNumber stationNumber,
                                     StationName stationName,
                                     StationAddress stationAddress,
                                     StationAddress stationAddress2,
                                     StationPostCode stationPostCode,
                                     StationCity stationCity,
                                     StationCountryRegionCode stationCountryRegionCode,
                                     StationLatitude stationLatitude,
                                     StationLongitude stationLongitude,
                                     StationSystem stationSystem)
        {
            return new Station(stationId,
                               sourceETag,
                               stationNumber,
                               stationName,
                               stationAddress,
                               stationAddress2,
                               stationPostCode,
                               stationCity,
                               stationCountryRegionCode,
                               stationLatitude,
                               stationLongitude,
                               stationSystem);
        }

    }
}
