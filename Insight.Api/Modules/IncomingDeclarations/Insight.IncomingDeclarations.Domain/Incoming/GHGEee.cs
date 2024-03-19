using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEee : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEee()
        {
            Value = decimal.Zero;
        }

        private GHGEee(decimal value)
        {
            Value = value;
        }

        public static GHGEee Create(decimal value)
        {
            return new GHGEee(value);
        }

        public static GHGEee Empty()
        {
            return new GHGEee();
        }
    }
}
