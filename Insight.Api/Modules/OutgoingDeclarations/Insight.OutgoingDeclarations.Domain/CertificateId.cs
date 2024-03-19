using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class CertificateId : ValueObject
    {
        public string Value { get; private set; }

        private CertificateId()
        {
            Value = string.Empty;;
        }

        private CertificateId(string value)
        {
            Value = value;
        }

        public static CertificateId Create(string value)
        {
            return new CertificateId(value);
        }

        public static CertificateId Empty()
        {
            return new CertificateId();
        }
    }
}