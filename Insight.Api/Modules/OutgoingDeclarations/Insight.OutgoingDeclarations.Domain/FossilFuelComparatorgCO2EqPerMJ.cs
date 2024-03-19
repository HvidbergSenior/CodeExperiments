using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class FossilFuelComparatorgCO2EqPerMJ : ValueObject
    {
        public decimal Value { get; private set; }

        private FossilFuelComparatorgCO2EqPerMJ()
        {
            Value = default;
        }

        private FossilFuelComparatorgCO2EqPerMJ(decimal value)
        {
            Value = value;
        }

        public static FossilFuelComparatorgCO2EqPerMJ Create(decimal company)
        {
            return new FossilFuelComparatorgCO2EqPerMJ(company);
        }

        public static FossilFuelComparatorgCO2EqPerMJ Empty()
        {
            return new FossilFuelComparatorgCO2EqPerMJ();
        }
    }
}