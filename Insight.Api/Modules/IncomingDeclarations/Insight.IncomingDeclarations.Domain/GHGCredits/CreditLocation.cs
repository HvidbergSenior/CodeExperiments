using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditLocation : ValueObject
    {
        public string Value { get; private set; }

        private CreditLocation()
        {
            Value = string.Empty;
        }

        private CreditLocation(string value)
        {
            Value = value;
        }

        public static CreditLocation Create(string value)
        {
            return new CreditLocation(value);
        }

        public static CreditLocation Empty()
        {
            return new CreditLocation();
        }
    }
}