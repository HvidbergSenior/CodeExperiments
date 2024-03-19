using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class DriverCardNumber : ValueObject
    {
        public string Value { get; private set; }

        private DriverCardNumber()
        {
            Value = string.Empty;
        }

        private DriverCardNumber(string value)
        {
            Value = value;
        }

        public static DriverCardNumber Create(string value)
        {
            return new DriverCardNumber(value);
        }

        public static DriverCardNumber Empty()
        {
            return new DriverCardNumber();
        }
    }
}