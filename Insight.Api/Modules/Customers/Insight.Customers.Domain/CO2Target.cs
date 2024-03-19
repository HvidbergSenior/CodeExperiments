using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain
{
    public sealed class CO2Target : ValueObject
    {
        public decimal Value { get; private set; }

        private CO2Target()
        {
            Value = default;
        }

        private CO2Target(decimal value)
        {
            Value = value;
        }

        public static CO2Target Create(decimal value)
        {
            return new CO2Target(value);
        }

        public static CO2Target Empty()
        {
            return new CO2Target();
        }
    }
}