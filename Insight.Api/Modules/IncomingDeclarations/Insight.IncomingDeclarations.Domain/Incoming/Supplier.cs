using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class Supplier : ValueObject
    {
        public string Value { get; private set; }

        private Supplier()
        {
            Value = string.Empty;;
        }

        private Supplier(string value)
        {
            Value = value;
        }

        public static Supplier Create(string value)
        {
            return new Supplier(value);
        }

        public static Supplier Empty()
        {
            return new Supplier();
        }
    }
}
