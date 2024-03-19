using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class AdminPrivileges : ValueObject
    {
        public bool Value { get; private set; }

        private AdminPrivileges()
        {
            Value = false;
        }

        private AdminPrivileges(bool value)
        {
            Value = value;
        }

        public static AdminPrivileges Create(bool value)
        {
            return new AdminPrivileges(value);
        }

        public static AdminPrivileges None()
        {
            return new AdminPrivileges(false);
        }
    }
}
