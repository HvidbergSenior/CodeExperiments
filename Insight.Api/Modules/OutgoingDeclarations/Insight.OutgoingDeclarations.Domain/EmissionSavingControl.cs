using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class EmissionSavingControl : ValueObject
    {
        public decimal Value { get; private set; }

        private EmissionSavingControl()
        {
            Value = default;
        }

        private EmissionSavingControl(decimal value)
        {
            Value = value;
        }

        public static EmissionSavingControl Create(decimal company)
        {
            return new EmissionSavingControl(company);
        }

        public static EmissionSavingControl Empty()
        {
            return new EmissionSavingControl();
        }
    }
}