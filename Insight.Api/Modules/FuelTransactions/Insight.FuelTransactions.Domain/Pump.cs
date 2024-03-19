using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Pump : ValueObject
    {
        public int Value { get; private set; }

        private Pump()
        {
            Value = default;
        }

        private Pump(int value)
        {
            Value = value;
        }

        public static Pump Create(int value)
        {
            return new Pump(value);
        }

        public static Pump Empty()
        {
            return new Pump();
        }
    }
}