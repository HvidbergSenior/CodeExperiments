using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Supplier : ValueObject
    {
        public string Value { get; private set; }

        private Supplier()
        {
            Value = string.Empty;
        }

        private Supplier(string value)
        {
            Value = value;
        }

        public static Supplier Create(string company)
        {
            return new Supplier(company);
        }

        public static Supplier Empty()
        {
            return new Supplier();
        }
    }
}