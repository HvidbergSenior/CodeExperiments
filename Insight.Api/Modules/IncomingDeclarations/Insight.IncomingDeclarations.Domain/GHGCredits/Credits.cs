using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class Credits : ValueObject
    {
        public decimal Value { get; private set; }

        private Credits()
        {
            Value = decimal.Zero;
        }

        private Credits(decimal value)
        {
            Value = value;
        }

        public static Credits Create(decimal value)
        {
            return new Credits(value);
        }

        public static Credits Empty()
        {
            return new Credits();
        }
    }
}
