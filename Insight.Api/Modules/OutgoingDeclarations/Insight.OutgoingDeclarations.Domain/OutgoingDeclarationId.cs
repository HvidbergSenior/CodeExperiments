using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class OutgoingDeclarationId : ValueObject
    {
        public Guid Value { get; private set; }

        private OutgoingDeclarationId()
        {
            Value = Guid.Empty;
        }

        private OutgoingDeclarationId(Guid value)
        {
            Value = value;
        }

        public static OutgoingDeclarationId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new OutgoingDeclarationId(value);
        }

        public static OutgoingDeclarationId Empty()
        {
            return new OutgoingDeclarationId(Guid.Empty);
        }
    }
}