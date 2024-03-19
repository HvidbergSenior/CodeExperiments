using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class Product : ValueObject
    {
        public string Value { get; private set; }

        private Product()
        {
            Value = string.Empty;
        }

        private Product(string value)
        {
            Value = value;
        }

        public static Product Create(string value)
        {
            return new Product(value);
        }

        public static Product Empty()
        {
            return new Product();
        }
    }
}
