using System.Text.Json.Serialization;

namespace Insight.Services.BusinessCentralConnector.Service.Station
{
    public class BusinessCentralStation : BusinessCentralEntity
    {
        [JsonPropertyName("@odata.etag")]
        public required string Etag { get; set; }
        [JsonPropertyName("StationNumber")]
        public required string StationNumber { get; set; }
        [JsonPropertyName("BFStation_Name")]
        public required string BFStationName { get; set; }
        [JsonPropertyName("BFStation_SiteOwner")]
        public required string BFStationSiteOwner { get; set; }
        [JsonPropertyName("BFStation_Public")]
        public required string BFStationPublic { get; set; }
        [JsonPropertyName("BFStation_System")]
        public required string BFStationSystem { get; set; }
        [JsonPropertyName("BFStation_LocationCode")]
        public required string BFStationLocationCode { get; set; }
        [JsonPropertyName("BFStation_Address")]
        public required string BFStationAddress { get; set; }
        [JsonPropertyName("BFStation_Address2")]
        public required string BFStationAddress2 { get; set; }
        [JsonPropertyName("BFStation_PostCode")]
        public required string BFStationPostCode { get; set; }
        [JsonPropertyName("BFStation_City")]
        public required string BFStationCity { get; set; }
        [JsonPropertyName("BFStation_CountryRegionCode")]
        public required string BFStationCountryRegionCode { get; set; }
        [JsonPropertyName("BFStation_Latitude")]
        public required string BFStationLatitude { get; set; }
        [JsonPropertyName("BFStation_Longitude")]
        public required string BFStationLongitude { get; set; }
        [JsonPropertyName("BFStation_County")]
        public string? BFStationCounty { get; set; }
        [JsonPropertyName("BFStation_DrivingInformation")]
        public string? BFStationDrivingInformation { get; set; }
        [JsonPropertyName("BFStation_SiteInformation")]
        public string? BFStationSiteInformation { get; set; }
        [JsonPropertyName("BFStation_B100")]
        public bool BFStationB100 { get; set; }
        [JsonPropertyName("Densitet_Netweight")]
        public bool BFStation_HVO100 { get; set; }
        [JsonPropertyName("BFStation_HVO100")]
        public bool BFStationDiesel { get; set; }
        [JsonPropertyName("BFStation_AdBlue")]
        public bool BFStationAdBlue { get; set; }
        [JsonPropertyName("BFStation_Active")]
        public string? BFStation_Active { get; set; }
        [JsonPropertyName("BFStation_HVODiesel")]
        public bool BFStationHVODiesel { get; set; }
        [JsonPropertyName("Operation_Information")]
        public string? OperationInformation { get; set; }
        [JsonPropertyName("Image_URL")]
        public string? ImageURL { get; set; }
    }
}