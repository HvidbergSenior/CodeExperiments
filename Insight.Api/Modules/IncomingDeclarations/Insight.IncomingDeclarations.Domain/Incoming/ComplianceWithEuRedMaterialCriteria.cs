using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ComplianceWithEuRedMaterialCriteria : ValueObject
    {
        public bool Value { get; private set; }

        private ComplianceWithEuRedMaterialCriteria()
        {
            Value = false;
        }

        private ComplianceWithEuRedMaterialCriteria(bool value)
        {
            Value = value;
        }

        public static ComplianceWithEuRedMaterialCriteria Create(bool value)
        {
            return new ComplianceWithEuRedMaterialCriteria(value);
        }

        public static ComplianceWithEuRedMaterialCriteria None()
        {
            return new ComplianceWithEuRedMaterialCriteria();
        }
    }
}
