using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ComplianceWithIsccMaterialCriteria : ValueObject
    {
        public bool Value { get; private set; }

        private ComplianceWithIsccMaterialCriteria()
        {
            Value = false;
        }

        private ComplianceWithIsccMaterialCriteria(bool value)
        {
            Value = value;
        }

        public static ComplianceWithIsccMaterialCriteria Create(bool value)
        {
            return new ComplianceWithIsccMaterialCriteria(value);
        }

        public static ComplianceWithIsccMaterialCriteria None()
        {
            return new ComplianceWithIsccMaterialCriteria();
        }
    }
}
