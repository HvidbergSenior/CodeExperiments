using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ProductVariant : ValueObject
    {
        public string Value { get; private set; }

        private ProductVariant()
        {
            Value = string.Empty;
        }

        private ProductVariant(string value)
        {
            Value = value;
        }

        public static ProductVariant Create(string value)
        {
            return new ProductVariant(value);
        }

        public static ProductVariant Empty()
        {
            return new ProductVariant();
        }
    }
}
