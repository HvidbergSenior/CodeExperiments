using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditEndDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private CreditEndDate()
        {
            Value = DateOnly.MinValue;
        }

        private CreditEndDate(DateOnly value)
        {
            Value = value;
        }

        public static CreditEndDate Create(DateOnly value)
        {
            return new CreditEndDate(value);
        }

        public static CreditEndDate Empty()
        {
            return new CreditEndDate();
        }

        public static CreditEndDate Today()
        {
            return new CreditEndDate(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
