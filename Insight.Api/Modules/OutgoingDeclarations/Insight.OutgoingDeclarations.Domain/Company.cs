using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Company : ValueObject
    {
        public string Value { get; private set; }

        private Company()
        {
            Value = string.Empty;;
        }

        private Company(string value)
        {
            Value = value;
        }

        public static Company Create(string value)
        {
            return new Company(value);
        }

        public static Company Empty()
        {
            return new Company();
        }
    }
}