using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class IncomingDeclarationId : ValueObject
    {
        public Guid Value { get; private set; }

        private IncomingDeclarationId()
        {
            Value = default;
        }

        private IncomingDeclarationId(Guid value)
        {
            Value = value;
        }

        public static IncomingDeclarationId Create(Guid value)
        {
            return new IncomingDeclarationId(value);
        }

        public static IncomingDeclarationId Empty()
        {
            return new IncomingDeclarationId();
        }
    }
}