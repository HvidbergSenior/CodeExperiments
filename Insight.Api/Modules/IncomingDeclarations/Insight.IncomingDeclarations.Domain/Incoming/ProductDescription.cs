using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ProductDescription : ValueObject
    {
        public string Value { get; private set; }

        private ProductDescription()
        {
            Value = string.Empty;
        }

        private ProductDescription(string value)
        {
            Value = value;
        }

        public static ProductDescription Create(string value)
        {
            return new ProductDescription(value);
        }

        public static ProductDescription Empty()
        {
            return new ProductDescription();
        }
    }
}
