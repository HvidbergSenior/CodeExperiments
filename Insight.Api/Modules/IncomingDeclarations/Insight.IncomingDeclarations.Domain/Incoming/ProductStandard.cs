using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ProductStandard : ValueObject
    {
        public string Value { get; private set; }

        private ProductStandard()
        {
            Value = string.Empty;
        }

        private ProductStandard(string value)
        {
            Value = value;
        }

        public static ProductStandard Create(string value)
        {
            return new ProductStandard(value);
        }

        public static ProductStandard Empty()
        {
            return new ProductStandard();
        }
    }
}
