
using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEec : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEec()
        {
            Value = decimal.Zero;
        }

        private GHGEec(decimal value)
        {
            Value = value;
        }

        public static GHGEec Create(decimal value)
        {
            return new GHGEec(value);
        }

        public static GHGEec Empty()
        {
            return new GHGEec();
        }
    }
}
