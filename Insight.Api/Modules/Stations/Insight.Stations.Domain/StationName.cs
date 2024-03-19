using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationName : ValueObject
    {
        public string Value { get; private set; }

        private StationName()
        {
            Value = string.Empty;
        }

        private StationName(string value)
        {
            Value = value;
        }

        public static StationName Create(string value)
        {
            return new StationName(value);
        }

        public static StationName Empty()
        {
            return new StationName();
        }
    }
}