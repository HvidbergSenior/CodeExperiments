using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
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

        public static Product Create(string company)
        {
            return new Product(company);
        }

        public static Product Empty()
        {
            return new Product();
        }
    }
}