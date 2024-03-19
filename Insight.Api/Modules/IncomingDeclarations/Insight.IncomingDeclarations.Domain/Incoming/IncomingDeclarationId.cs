using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class IncomingDeclarationId : ValueObject
    {
        public Guid Value { get; private set; }

        private IncomingDeclarationId()
        {
            Value = Guid.Empty;
        }

        private IncomingDeclarationId(Guid value)
        {
            Value = value;
        }

        public static IncomingDeclarationId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new IncomingDeclarationId(value);
        }

        public static IncomingDeclarationId Empty()
        {
            return new IncomingDeclarationId(Guid.Empty);
        }
    }
}
