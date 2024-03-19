using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEp : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEp()
        {
            Value = decimal.Zero;
        }

        private GHGEp(decimal value)
        {
            Value = value;
        }

        public static GHGEp Create(decimal value)
        {
            return new GHGEp(value);
        }

        public static GHGEp Empty()
        {
            return new GHGEp();
        }
    }
}
