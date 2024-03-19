using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGgCO2eqPerMJ : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGgCO2eqPerMJ()
        {
            Value = decimal.Zero;
        }

        private GHGgCO2eqPerMJ(decimal value)
        {
            Value = value;
        }

        public static GHGgCO2eqPerMJ Create(decimal value)
        {
            return new GHGgCO2eqPerMJ(value);
        }

        public static GHGgCO2eqPerMJ Empty()
        {
            return new GHGgCO2eqPerMJ();
        }
    }
}
