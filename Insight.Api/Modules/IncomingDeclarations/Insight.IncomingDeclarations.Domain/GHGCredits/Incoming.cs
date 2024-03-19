using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class Incoming : ValueObject
    {
        public decimal Value { get; private set; }

        private Incoming()
        {
            Value = decimal.Zero;
        }

        private Incoming(decimal value)
        {
            Value = value;
        }

        public static Incoming Create(decimal value)
        {
            return new Incoming(value);
        }

        public static Incoming Empty()
        {
            return new Incoming();
        }
    }
}