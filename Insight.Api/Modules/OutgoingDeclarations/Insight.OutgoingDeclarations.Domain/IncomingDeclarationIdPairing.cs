using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class IncomingDeclarationIdPairing : ValueObject
    {
        public IncomingDeclarationId IncomingDeclarationId { get; set; }
        public Quantity Quantity { get; set; }
        public BatchId BatchId { get; set; }

        private IncomingDeclarationIdPairing()
        {
            IncomingDeclarationId = IncomingDeclarationId.Empty();
            Quantity = Quantity.Empty();
            BatchId = BatchId.Empty();
        }

        private IncomingDeclarationIdPairing(IncomingDeclarationId incomingDeclarationId, Quantity quantity, BatchId batchId)
        {
            IncomingDeclarationId = incomingDeclarationId;
            Quantity = quantity;
            BatchId = batchId;
        }

        public static IncomingDeclarationIdPairing Create(IncomingDeclarationId incomingDeclarationId, Quantity quantity, BatchId batchId)
        {
            return new IncomingDeclarationIdPairing(incomingDeclarationId, quantity, batchId);
        }

        public static IncomingDeclarationIdPairing Empty()
        {
            return new IncomingDeclarationIdPairing();
        }
    }
}