using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
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

        public static ProductionCountry Create(string company)
        {
            return new ProductionCountry(company);
        }

        public static ProductionCountry Empty()
        {
            return new ProductionCountry();
        }
    }
}