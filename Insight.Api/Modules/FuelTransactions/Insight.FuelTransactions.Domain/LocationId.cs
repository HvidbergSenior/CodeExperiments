using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class LocationId : ValueObject
    {
        public string Value { get; private set; }

        private LocationId()
        {
            Value = string.Empty;
        }

        private LocationId(string value)
        {
            Value = value;
        }

        public static LocationId Create(string value)
        {
            return new LocationId(value);
        }

        public static LocationId Empty()
        {
            return new LocationId();
        }
    }
}