using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationLongitude : ValueObject
    {
        public double Value { get; private set; }

        private StationLongitude()
        {
            Value = double.MinValue;
        }

        private StationLongitude(double value)
        {
            Value = value;
        }

        public static StationLongitude Create(double value)
        {
            return new StationLongitude(value);
        }

        public static StationLongitude Empty()
        {
            return new StationLongitude();
        }
    }
}