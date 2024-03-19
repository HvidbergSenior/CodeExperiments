using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class HasFuelConsumptionAccess : ValueObject
    {
        public bool Value { get; private set; }

        private HasFuelConsumptionAccess()
        {
            Value = false;
        }

        private HasFuelConsumptionAccess(bool value)
        {
            Value = value;
        }

        public static HasFuelConsumptionAccess Create(bool value)
        {
            return new HasFuelConsumptionAccess(value);
        }

        public static HasFuelConsumptionAccess None()
        {
            return new HasFuelConsumptionAccess(false);
        }
    }
}
