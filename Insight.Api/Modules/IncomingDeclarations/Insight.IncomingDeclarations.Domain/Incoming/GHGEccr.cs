using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEccr : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEccr()
        {
            Value = decimal.Zero;
        }

        private GHGEccr(decimal value)
        {
            Value = value;
        }

        public static GHGEccr Create(decimal value)
        {
            return new GHGEccr(value);
        }

        public static GHGEccr Empty()
        {
            return new GHGEccr();
        }
    }
}
