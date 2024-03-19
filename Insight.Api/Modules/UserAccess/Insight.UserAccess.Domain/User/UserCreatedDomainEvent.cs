using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class UserCreatedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        public AdminPrivileges AdminPrivileges { get; private set; }
        public Blocked Blocked {get; private set; }

        private UserCreatedDomainEvent(UserId userId, UserName userName, Email email, AdminPrivileges adminPrivileges, Blocked blocked)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            AdminPrivileges = adminPrivileges;
            Blocked = blocked;
        }

        public static UserCreatedDomainEvent Create(UserId userId, UserName userName, Email email, AdminPrivileges adminPrivileges, Blocked blocked)
        {
            return new UserCreatedDomainEvent(userId, userName, email, adminPrivileges, blocked);
        }
    }
}
