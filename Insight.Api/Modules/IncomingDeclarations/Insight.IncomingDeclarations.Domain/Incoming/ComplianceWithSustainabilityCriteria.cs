using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ComplianceWithSustainabilityCriteria : ValueObject
    {
        public bool Value { get; private set; }

        private ComplianceWithSustainabilityCriteria()
        {
            Value = false;
        }

        private ComplianceWithSustainabilityCriteria(bool value)
        {
            Value = value;
        }

        public static ComplianceWithSustainabilityCriteria Create(bool value)
        {
            return new ComplianceWithSustainabilityCriteria(value);
        }

        public static ComplianceWithSustainabilityCriteria None()
        {
            return new ComplianceWithSustainabilityCriteria();
        }
    }
}
