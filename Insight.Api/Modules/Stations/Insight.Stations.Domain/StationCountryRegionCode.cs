using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationCountryRegionCode : ValueObject
    {
        public string Value { get; private set; }

        private StationCountryRegionCode()
        {
            Value = string.Empty;
        }

        private StationCountryRegionCode(string value)
        {
            Value = value;
        }

        public static StationCountryRegionCode Create(string value)
        {
            return new StationCountryRegionCode(value);
        }

        public static StationCountryRegionCode Empty()
        {
            return new StationCountryRegionCode();
        }
    }
}