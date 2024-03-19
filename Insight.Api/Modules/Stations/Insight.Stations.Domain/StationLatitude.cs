using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationLatitude : ValueObject
    {
        public double Value { get; private set; }

        private StationLatitude()
        {
            Value = double.MinValue;
        }

        private StationLatitude(double value)
        {
            Value = value;
        }

        public static StationLatitude Create(double value)
        {
            return new StationLatitude(value);
        }

        public static StationLatitude Empty()
        {
            return new StationLatitude();
        }
    }
}