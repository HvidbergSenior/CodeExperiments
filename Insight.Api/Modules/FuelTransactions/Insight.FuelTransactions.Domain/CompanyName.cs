using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class CompanyName : ValueObject
    {
        public string Value { get; private set; }

        private CompanyName()
        {
            Value = string.Empty;
        }

        private CompanyName(string value)
        {
            Value = value;
        }

        public static CompanyName Create(string value)
        {
            return new CompanyName(value);
        }

        public static CompanyName Empty()
        {
            return new CompanyName();
        }
    }
}