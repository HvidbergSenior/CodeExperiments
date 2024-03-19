using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class UserUpdatedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public AdminPrivileges AdminPrivileges { get; private set; }
        public Blocked Blocked {get; private set;}
        public IEnumerable<CustomerPermissionGroup> CustomerPermissions { get; private set; }

        private UserUpdatedDomainEvent(UserId userId, UserName userName, Email email, AdminPrivileges adminPrivileges, Blocked blocked, IEnumerable<CustomerPermissionGroup> customerPermissions, FirstName firstName, LastName lastName)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            AdminPrivileges = adminPrivileges;
            Blocked = blocked;
            CustomerPermissions = customerPermissions;
            FirstName = firstName;
            LastName = lastName;
        }

        public static UserUpdatedDomainEvent Create(UserId userId, UserName userName, Email email, AdminPrivileges adminPrivileges, Blocked blocked, IEnumerable<CustomerPermissionGroup> customerPermissions, FirstName firstName, LastName lastName)
        {
            return new UserUpdatedDomainEvent(userId, userName, email, adminPrivileges, blocked, customerPermissions, firstName, lastName);
        }
    }
}