using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain.SustainabilityReports
{
    public sealed class ItemCount : ValueObject
    {
        public int Value { get; private set; }

        private ItemCount()
        {
            Value = default;
        }

        private ItemCount(int value)
        {
            Value = value;
        }

        public static ItemCount Create(int value)
        {
            return new ItemCount(value);
        }

        public static ItemCount Empty()
        {
            return new ItemCount();
        }
    }
}