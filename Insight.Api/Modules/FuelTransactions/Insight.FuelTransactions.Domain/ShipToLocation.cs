using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class ShipToLocation : ValueObject
    {
        public string Value { get; private set; }

        private ShipToLocation()
        {
            Value = string.Empty;
        }

        private ShipToLocation(string value)
        {
            Value = value;
        }

        public static ShipToLocation Create(string value)
        {
            return new ShipToLocation(value);
        }

        public static ShipToLocation Empty()
        {
            return new ShipToLocation();
        }
    }
}
