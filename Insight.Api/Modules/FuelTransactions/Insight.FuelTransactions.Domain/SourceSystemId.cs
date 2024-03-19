using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class SourceSystemId : ValueObject
    {
        public Guid Value { get; private set; }

        private SourceSystemId()
        {
            Value = Guid.Empty;
        }

        private SourceSystemId(Guid value)
        {
            Value = value;
        }

        public static SourceSystemId Create(Guid value)
        {
            return new SourceSystemId(value);
        }

        public static SourceSystemId Empty()
        {
            return new SourceSystemId();
        }
    }
}
