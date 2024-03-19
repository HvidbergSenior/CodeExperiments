using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class IncomingDeclarationUploadId : ValueObject
    {
        public Guid Value { get; private set; }
        
        private IncomingDeclarationUploadId()
        {
            Value = Guid.Empty;
        }

        private IncomingDeclarationUploadId(Guid value)
        {
            Value = value;
        }

        public static IncomingDeclarationUploadId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new IncomingDeclarationUploadId(value);
        }

        public static IncomingDeclarationUploadId Empty()
        {
            return new IncomingDeclarationUploadId(Guid.Empty);
        }
    }
}
