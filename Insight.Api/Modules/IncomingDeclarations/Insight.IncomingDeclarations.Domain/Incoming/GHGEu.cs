using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEu : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEu()
        {
            Value = decimal.Zero;
        }

        private GHGEu(decimal value)
        {
            Value = value;
        }

        public static GHGEu Create(decimal value)
        {
            return new GHGEu(value);
        }

        public static GHGEu Empty()
        {
            return new GHGEu();
        }
    }
}
