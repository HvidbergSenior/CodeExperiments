using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain.Products
{
    public sealed class Description : ValueObject
    {
        public string Value { get; private set; }

        private Description()
        {
            Value = string.Empty;
        }

        private Description(string value)
        {
            Value = value;
        }

        public static Description Create(string value)
        {
            return new Description(value);
        }

        public static Description Empty()
        {
            return new Description();
        }
    }
}