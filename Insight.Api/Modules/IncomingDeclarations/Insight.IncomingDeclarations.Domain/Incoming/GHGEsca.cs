using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEsca : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEsca()
        {
            Value = decimal.Zero;
        }

        private GHGEsca(decimal value)
        {
            Value = value;
        }

        public static GHGEsca Create(decimal value)
        {
            return new GHGEsca(value);
        }

        public static GHGEsca Empty()
        {
            return new GHGEsca();
        }
    }
}
