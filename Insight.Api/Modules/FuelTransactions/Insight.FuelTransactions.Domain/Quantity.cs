using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class Quantity : ValueObject
    {
        public decimal Value { get; private set; }

        private Quantity()
        {
            Value = default;
        }

        private Quantity(decimal value)
        {
            Value = value;
        }

        public static Quantity Create(decimal value)
        {
            return new Quantity(value);
        }

        public static Quantity Empty()
        {
            return new Quantity();
        }
    }
}