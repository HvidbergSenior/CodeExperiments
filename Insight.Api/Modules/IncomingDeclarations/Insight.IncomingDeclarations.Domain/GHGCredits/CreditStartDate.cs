using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditStartDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private CreditStartDate()
        {
            Value = DateOnly.MinValue;
        }

        private CreditStartDate(DateOnly value)
        {
            Value = value;
        }

        public static CreditStartDate Create(DateOnly value)
        {
            return new CreditStartDate(value);
        }

        public static CreditStartDate Empty()
        {
            return new CreditStartDate();
        }

        public static CreditStartDate Today()
        {
            return new CreditStartDate(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
