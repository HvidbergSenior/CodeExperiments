using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class CountryOfOrigin : ValueObject
    {
        public string Value { get; private set; }

        private CountryOfOrigin()
        {
            Value = string.Empty;
        }

        private CountryOfOrigin(string value)
        {
            Value = value;
        }

        public static CountryOfOrigin Create(string value)
        {
            return new CountryOfOrigin(value);
        }

        public static CountryOfOrigin Empty()
        {
            return new CountryOfOrigin();
        }
    }
}
