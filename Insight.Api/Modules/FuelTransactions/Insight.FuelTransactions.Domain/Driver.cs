using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Driver : ValueObject
    {
        public string Value { get; private set; }

        private Driver()
        {
            Value = string.Empty;
        }

        private Driver(string value)
        {
            Value = value;
        }

        public static Driver Create(string value)
        {
            return new Driver(value);
        }

        public static Driver Empty()
        {
            return new Driver();
        }
    }
}
