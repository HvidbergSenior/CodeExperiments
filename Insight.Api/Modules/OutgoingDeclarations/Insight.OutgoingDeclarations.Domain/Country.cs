using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Country : ValueObject
    {
        public string Value { get; private set; }

        private Country()
        {
            Value = string.Empty;;
        }

        private Country(string value)
        {
            Value = value;
        }

        public static Country Create(string value)
        {
            return new Country(value);
        }

        public static Country Empty()
        {
            return new Country();
        }
    }
}
