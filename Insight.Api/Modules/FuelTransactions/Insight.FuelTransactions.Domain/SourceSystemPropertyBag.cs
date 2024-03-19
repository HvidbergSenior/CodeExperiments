using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class SourceSystemPropertyBag : ValueObject
    {
        public string Value { get; private set; }

        private SourceSystemPropertyBag()
        {
            Value = string.Empty;
        }

        private SourceSystemPropertyBag(string value)
        {
            Value = value;
        }

        public static SourceSystemPropertyBag Create(string value)
        {
            return new SourceSystemPropertyBag(value);
        }

        public static SourceSystemPropertyBag Empty()
        {
            return new SourceSystemPropertyBag();
        }
    }
}
