using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class EnergyContent : ValueObject
    {
        public decimal Value { get; private set; }

        private EnergyContent()
        {
            Value = default;
        }

        private EnergyContent(decimal value)
        {
            Value = value;
        }

        public static EnergyContent Create(decimal company)
        {
            return new EnergyContent(company);
        }

        public static EnergyContent Empty()
        {
            return new EnergyContent();
        }
    }
}