using Insight.BuildingBlocks.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class AllocationAssignment : ValueObject
    {
        public IncomingDeclarationId IncomingDeclarationId { get; private set; } = IncomingDeclarationId.Empty();

        public Quantity Volume { get; private set; } = Quantity.Empty();

        private AllocationAssignment(IncomingDeclarationId incomingDeclarationId, Quantity volume)
        {
            Volume = volume;
            IncomingDeclarationId = incomingDeclarationId;
        }

        public static AllocationAssignment Create(IncomingDeclarationId incomingDeclarationId, Quantity volume)
        {
            return new AllocationAssignment(incomingDeclarationId, volume);
        }
    }
}
