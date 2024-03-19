using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class GhgReduction : ValueObject
    {
        public decimal Value { get; private set; }

        private GhgReduction()
        {
            Value = default;
        }

        private GhgReduction(decimal value)
        {
            Value = value;
        }

        public static GhgReduction Create(decimal value)
        {
            return new GhgReduction(value);
        }

        public static GhgReduction Empty()
        {
            return new GhgReduction();
        }
    }
}