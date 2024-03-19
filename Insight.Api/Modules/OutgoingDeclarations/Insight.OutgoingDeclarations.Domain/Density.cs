using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Density : ValueObject
    {
        public decimal Value { get; private set; }

        private Density()
        {
            Value = default;
        }

        private Density(decimal value)
        {
            Value = value;
        }

        public static Density Create(decimal value)
        {
            return new Density(value);
        }

        public static Density Empty()
        {
            return new Density();
        }
    }
}