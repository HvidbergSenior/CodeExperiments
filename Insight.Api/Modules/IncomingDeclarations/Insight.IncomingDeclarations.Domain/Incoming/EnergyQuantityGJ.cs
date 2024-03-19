using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class EnergyQuantityGJ : ValueObject
    {
        public decimal Value { get; private set; }

        private EnergyQuantityGJ()
        {
            Value = decimal.Zero;
        }

        private EnergyQuantityGJ(decimal value)
        {
            Value = value;
        }

        public static EnergyQuantityGJ Create(decimal value)
        {
            return new EnergyQuantityGJ(value);
        }

        public static EnergyQuantityGJ Empty()
        {
            return new EnergyQuantityGJ();
        }
    }
}
