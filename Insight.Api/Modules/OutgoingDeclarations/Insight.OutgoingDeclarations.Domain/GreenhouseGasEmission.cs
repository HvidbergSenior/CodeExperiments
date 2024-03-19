using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class GreenhouseGasEmission : ValueObject
    {
        public decimal Value { get; private set; }

        private GreenhouseGasEmission()
        {
            Value = default;
        }

        private GreenhouseGasEmission(decimal value)
        {
            Value = value;
        }

        public static GreenhouseGasEmission Create(decimal company)
        {
            return new GreenhouseGasEmission(company);
        }

        public static GreenhouseGasEmission Empty()
        {
            return new GreenhouseGasEmission();
        }
    }
}