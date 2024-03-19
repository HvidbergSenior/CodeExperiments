using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Mt : ValueObject
    {
        public decimal Value { get; private set; }

        private Mt()
        {
            Value = default;
        }

        private Mt(decimal value)
        {
            Value = value;
        }

        public static Mt Create(decimal value)
        {
            return new Mt(value);
        }

        public static Mt Empty()
        {
            return new Mt();
        }
    }
}