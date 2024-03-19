using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class Allocations : ValueObject
    {
        public Dictionary<Guid, decimal> Value { get; private set; } = new Dictionary<Guid, decimal>();
        public decimal TotalAllocatedVolume => Value.Sum(x => x.Value);

        private Allocations()
        {
            // Intentionally left empty
        }

        private Allocations(Dictionary<Guid, decimal> value)
        {
            Value = value;
        }

        public static Allocations Create(Dictionary<Guid, decimal> value)
        {
            return new Allocations(value);
        }

        public static Allocations Empty()
        {
            return new Allocations();
        }
    }
}