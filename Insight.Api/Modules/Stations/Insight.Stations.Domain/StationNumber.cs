using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationNumber : ValueObject
    {
        public string Value { get; private set; }

        private StationNumber()
        {
            Value = string.Empty;
        }

        private StationNumber(string value)
        {
            Value = value;
        }

        public static StationNumber Create(string value)
        {
            return new StationNumber(value);
        }

        public static StationNumber Empty()
        {
            return new StationNumber();
        }
    }
}