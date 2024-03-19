using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class FossilFuelComparatorgCO2eqPerMJ : ValueObject
    {
        public decimal Value { get; private set; }

        private FossilFuelComparatorgCO2eqPerMJ()
        {
            Value = decimal.Zero;
        }

        private FossilFuelComparatorgCO2eqPerMJ(decimal value)
        {
            Value = value;
        }

        public static FossilFuelComparatorgCO2eqPerMJ Create(decimal value)
        {
            return new FossilFuelComparatorgCO2eqPerMJ(value);
        }

        public static FossilFuelComparatorgCO2eqPerMJ Empty()
        {
            return new FossilFuelComparatorgCO2eqPerMJ();
        }
    }
}
