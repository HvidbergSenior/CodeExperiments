using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class EnergyContentMJ : ValueObject
    {
        public decimal Value { get; private set; }

        private EnergyContentMJ()
        {
            Value = decimal.Zero;
        }

        private EnergyContentMJ(decimal value)
        {
            Value = value;
        }

        public static EnergyContentMJ Create(decimal value)
        {
            return new EnergyContentMJ(value);
        }

        public static EnergyContentMJ Empty()
        {
            return new EnergyContentMJ();
        }
    }
}
