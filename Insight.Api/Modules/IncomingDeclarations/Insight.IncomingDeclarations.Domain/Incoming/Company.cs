using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
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

        public static Company Create(string company)
        {
            return new Company(company);
        }

        public static Company Empty()
        {
            return new Company();
        }
    }
}