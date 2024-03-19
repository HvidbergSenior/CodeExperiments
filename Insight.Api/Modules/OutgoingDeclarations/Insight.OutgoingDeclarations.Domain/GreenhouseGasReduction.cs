using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class GreenhouseGasReduction : ValueObject
    {
        public decimal Value { get; private set; }

        private GreenhouseGasReduction()
        {
            Value = default;
        }

        private GreenhouseGasReduction(decimal value)
        {
            Value = value;
        }

        public static GreenhouseGasReduction Create(decimal company)
        {
            return new GreenhouseGasReduction(company);
        }

        public static GreenhouseGasReduction Empty()
        {
            return new GreenhouseGasReduction();
        }
    }
}