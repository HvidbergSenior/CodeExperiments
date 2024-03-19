using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Percentage : ValueObject
    {
        public decimal Value { get; private set; }

        private Percentage()
        {
            Value = default;
        }

        private Percentage(decimal value)
        {
            Value = value;
        }

        public static Percentage Create(decimal value)
        {
            return new Percentage(value);
        }

        public static Percentage Empty()
        {
            return new Percentage();
        }
    }
}