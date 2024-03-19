using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Odometer : ValueObject
    {
        public int Value { get; private set; }

        private Odometer()
        {
            Value = default;
        }

        private Odometer(int value)
        {
            Value = value;
        }

        public static Odometer Create(int value)
        {
            return new Odometer(value);
        }

        public static Odometer Empty()
        {
            return new Odometer();
        }
    }
}