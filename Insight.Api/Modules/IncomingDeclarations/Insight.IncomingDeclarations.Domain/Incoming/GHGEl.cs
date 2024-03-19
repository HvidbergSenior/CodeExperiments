using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEl : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEl()
        {
            Value = decimal.Zero;
        }

        private GHGEl(decimal value)
        {
            Value = value;
        }

        public static GHGEl Create(decimal value)
        {
            return new GHGEl(value);
        }

        public static GHGEl Empty()
        {
            return new GHGEl();
        }
    }
}
