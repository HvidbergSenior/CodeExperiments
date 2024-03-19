using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class CertificationSystem : ValueObject
    {
        public string Value { get; private set; }

        private CertificationSystem()
        {
            Value = string.Empty;;
        }

        private CertificationSystem(string value)
        {
            Value = value;
        }

        public static CertificationSystem Create(string value)
        {
            return new CertificationSystem(value);
        }

        public static CertificationSystem Empty()
        {
            return new CertificationSystem();
        }
    }
}
