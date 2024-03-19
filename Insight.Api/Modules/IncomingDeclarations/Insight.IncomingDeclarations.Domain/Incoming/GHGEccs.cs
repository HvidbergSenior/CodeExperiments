using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEccs : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEccs()
        {
            Value = decimal.Zero;
        }

        private GHGEccs(decimal value)
        {
            Value = value;
        }

        public static GHGEccs Create(decimal value)
        {
            return new GHGEccs(value);
        }

        public static GHGEccs Empty()
        {
            return new GHGEccs();
        }
    }
}
