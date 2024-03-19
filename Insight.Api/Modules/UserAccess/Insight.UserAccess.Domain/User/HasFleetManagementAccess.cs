using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class HasFleetManagementAccess : ValueObject
    {
        public bool Value { get; private set; }

        private HasFleetManagementAccess()
        {
            Value = false;
        }

        private HasFleetManagementAccess(bool value)
        {
            Value = value;
        }

        public static HasFleetManagementAccess Create(bool value)
        {
            return new HasFleetManagementAccess(value);
        }

        public static HasFleetManagementAccess None()
        {
            return new HasFleetManagementAccess(false);
        }
    }
}
