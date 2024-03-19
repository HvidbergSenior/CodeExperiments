using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditId : ValueObject
    {
        public Guid Value { get; private set; }

        private CreditId()
        {
            Value = Guid.Empty;
        }

        private CreditId(Guid value)
        {
            Value = value;
        }

        public static CreditId Create(Guid value)
        {
            return new CreditId(value);
        }

        public static CreditId Empty()
        {
            return new CreditId();
        }
    }
}