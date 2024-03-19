using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class BatchId : ValueObject
    {
        public long Value { get; private set; }

        private BatchId()
        {
            Value = default;;
        }

        private BatchId(long value)
        {
            Value = value;
        }

        public static BatchId Create(long value)
        {
            return new BatchId(value);
        }

        public static BatchId Empty()
        {
            return new BatchId();
        }
    }
}