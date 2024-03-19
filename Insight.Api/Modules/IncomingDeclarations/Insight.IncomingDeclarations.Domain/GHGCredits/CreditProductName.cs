using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditProductName : ValueObject
    {
        public string Value { get; private set; }

        private CreditProductName()
        {
            Value = string.Empty;
        }

        private CreditProductName(string value)
        {
            Value = value;
        }

        public static CreditProductName Create(string value)
        {
            return new CreditProductName(value);
        }

        public static CreditProductName Empty()
        {
            return new CreditProductName();
        }
    }
}