using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain.SustainabilityReports
{
    public sealed class Quantity : ValueObject
    {
        public int Value { get; private set; }

        private Quantity()
        {
            Value = default;
        }

        private Quantity(int value)
        {
            Value = value;
        }

        public static Quantity Create(int value)
        {
            return new Quantity(value);
        }

        public static Quantity Empty()
        {
            return new Quantity();
        }
    }
}