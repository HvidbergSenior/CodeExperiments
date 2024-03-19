namespace Insight.BuildingBlocks.Domain
{
    public sealed class ProductName : ValueObject
    {
        public string Value { get; private set; }

        private ProductName()
        {
            Value = string.Empty;
        }

        private ProductName(string value)
        {
            Value = value;
        }

        public static ProductName Create(string value)
        {
            return new ProductName(value);
        }

        public static ProductName Empty()
        {
            return new ProductName();
        }
    }
}
