using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditCompanyName : ValueObject
    {
        public string Value { get; private set; }

        private CreditCompanyName()
        {
            Value = string.Empty;
        }

        private CreditCompanyName(string value)
        {
            Value = value;
        }

        public static CreditCompanyName Create(string value)
        {
            return new CreditCompanyName(value);
        }

        public static CreditCompanyName Empty()
        {
            return new CreditCompanyName();
        }
    }
}