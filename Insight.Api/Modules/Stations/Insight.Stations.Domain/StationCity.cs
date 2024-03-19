using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationCity : ValueObject
    {
        public string Value { get; private set; }

        private StationCity()
        {
            Value = string.Empty;
        }

        private StationCity(string value)
        {
            Value = value;
        }

        public static StationCity Create(string value)
        {
            return new StationCity(value);
        }

        public static StationCity Empty()
        {
            return new StationCity();
        }
    }
}