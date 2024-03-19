using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain.Products
{
    public sealed class ProductNumber : ValueObject
    {
        public string Value { get; private set; }

        private ProductNumber()
        {
            Value = string.Empty;
        }

        private ProductNumber(string value)
        {
            Value = value;
        }

        public static ProductNumber Create(string value)
        {
            return new ProductNumber(value);
        }

        public static ProductNumber Empty()
        {
            return new ProductNumber();
        }
    }
}

