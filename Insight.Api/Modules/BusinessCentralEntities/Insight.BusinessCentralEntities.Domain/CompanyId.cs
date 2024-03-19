using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain
{
    public sealed class CompanyId : ValueObject
    {
        public Guid Value { get; private set; }

        private CompanyId()
        {
            Value = Guid.Empty;
        }

        private CompanyId(Guid value)
        {
            Value = value;
        }

        public static CompanyId Create(Guid value)
        {
            return new CompanyId(value);
        }

        public static CompanyId Empty()
        {
            return new CompanyId();
        }
    }
}
