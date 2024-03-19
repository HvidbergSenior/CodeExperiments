using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEtd : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEtd()
        {
            Value = decimal.Zero;
        }

        private GHGEtd(decimal value)
        {
            Value = value;
        }

        public static GHGEtd Create(decimal value)
        {
            return new GHGEtd(value);
        }

        public static GHGEtd Empty()
        {
            return new GHGEtd();
        }
    }
}
