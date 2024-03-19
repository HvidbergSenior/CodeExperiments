using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ProductionCountry : ValueObject
    {
        public string Value { get; private set; }

        private ProductionCountry()
        {
            Value = string.Empty;
        }

        private ProductionCountry(string value)
        {
            Value = value;
        }

        public static ProductionCountry Create(string value)
        {
            return new ProductionCountry(value);
        }

        public static ProductionCountry Empty()
        {
            return new ProductionCountry();
        }
    }
}
