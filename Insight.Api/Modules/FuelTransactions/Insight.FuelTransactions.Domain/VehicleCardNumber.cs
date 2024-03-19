using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class VehicleCardNumber : ValueObject
    {
        public string Value { get; private set; }

        private VehicleCardNumber()
        {
            Value = string.Empty;
        }

        private VehicleCardNumber(string value)
        {
            Value = value;
        }

        public static VehicleCardNumber Create(string value)
        {
            return new VehicleCardNumber(value);
        }

        public static VehicleCardNumber Empty()
        {
            return new VehicleCardNumber();
        }
    }
}