using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Liter : ValueObject
    {
        public decimal Value { get; private set; }

        private Liter()
        {
            Value = default;
        }

        private Liter(decimal value)
        {
            Value = value;
        }

        public static Liter Create(decimal value)
        {
            return new Liter(value);
        }

        public static Liter Empty()
        {
            return new Liter();
        }
    }
}