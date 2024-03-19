using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class SupplierCertificateNumber : ValueObject
    {
        public string Value { get; private set; }

        private SupplierCertificateNumber()
        {
            Value = string.Empty;
        }

        private SupplierCertificateNumber(string value)
        {
            Value = value;
        }

        public static SupplierCertificateNumber Create(string value)
        {
            return new SupplierCertificateNumber(value);
        }

        public static SupplierCertificateNumber Empty()
        {
            return new SupplierCertificateNumber();
        }
    }
}
