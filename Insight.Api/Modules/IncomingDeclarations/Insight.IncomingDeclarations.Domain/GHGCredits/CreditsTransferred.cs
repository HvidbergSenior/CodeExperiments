using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditsTransferred : ValueObject
    {
        public decimal Value { get; private set; }

        private CreditsTransferred()
        {
            Value = decimal.Zero;
        }

        private CreditsTransferred(decimal value)
        {
            Value = value;
        }

        public static CreditsTransferred Create(decimal value)
        {
            return new CreditsTransferred(value);
        }

        public static CreditsTransferred Empty()
        {
            return new CreditsTransferred();
        }
    }
}