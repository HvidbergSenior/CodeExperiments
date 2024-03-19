using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationPostCode : ValueObject
    {
        public string Value { get; private set; }

        private StationPostCode()
        {
            Value = string.Empty;
        }

        private StationPostCode(string value)
        {
            Value = value;
        }

        public static StationPostCode Create(string value)
        {
            return new StationPostCode(value);
        }

        public static StationPostCode Empty()
        {
            return new StationPostCode();
        }
    }
}