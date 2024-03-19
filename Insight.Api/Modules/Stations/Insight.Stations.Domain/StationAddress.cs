using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationAddress : ValueObject
    {
        public string Value { get; private set; }

        private StationAddress()
        {
            Value = string.Empty;
        }

        private StationAddress(string value)
        {
            Value = value;
        }

        public static StationAddress Create(string value)
        {
            return new StationAddress(value);
        }

        public static StationAddress Empty()
        {
            return new StationAddress();
        }
    }
}