using Insight.BuildingBlocks.Domain;
namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditCompanyId : ValueObject
    {
        public Guid Value { get; private set; }

        private CreditCompanyId()
        {
            Value = Guid.Empty;
        }

        private CreditCompanyId(Guid value)
        {
            Value = value;
        }

        public static CreditCompanyId Create(Guid value)
        {
            return new CreditCompanyId(value);
        }

        public static CreditCompanyId Empty()
        {
            return new CreditCompanyId();
        }
    }
}