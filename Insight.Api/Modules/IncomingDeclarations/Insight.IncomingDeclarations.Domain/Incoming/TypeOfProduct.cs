using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class TypeOfProduct : ValueObject
    {
        public string Value { get; private set; }

        private TypeOfProduct()
        {
            Value = string.Empty;
        }

        private TypeOfProduct(string value)
        {
            Value = value;
        }

        public static TypeOfProduct Create(string value)
        {
            return new TypeOfProduct(value);
        }

        public static TypeOfProduct Empty()
        {
            return new TypeOfProduct();
        }
    }
}
