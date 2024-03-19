using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class User : Entity
    {
        public UserId UserId { get; private set; }
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public AdminPrivileges AdminPrivileges { get; private set; }
        public string[] CustomerIds => CustomerPermissions.Select(x => x.CustomerId.Value.ToString()).Distinct().ToArray();
        public PasswordHash PasswordHash { get; private set; }
        public SecurityStamp SecurityStamp { get; private set; }
        public List<CustomerPermissionGroup> CustomerPermissions { get; private set; }
        public Blocked Blocked { get; private set; }

        private User()
        {
            UserId = UserId.Empty();
            UserName = UserName.Empty();
            Email = Email.Empty();
            PasswordHash = PasswordHash.Empty();
            SecurityStamp = SecurityStamp.Empty();
            AdminPrivileges = AdminPrivileges.None();
            CustomerPermissions = new();
            Blocked = Blocked.Create(false);
            FirstName = FirstName.Empty();
            LastName = LastName.Empty();
        }

        private User(UserId userId, UserName userName, Email email, PasswordHash passwordHash, SecurityStamp securityStamp, AdminPrivileges adminPrivileges, IEnumerable<CustomerPermissionGroup> customerPermissions, Blocked blocked, FirstName firstName, LastName lastName)
        {
            Id = userId.Value;
            UserId = userId;
            UserName = userName;
            PasswordHash = passwordHash;
            SecurityStamp = securityStamp;
            Email = email;
            AdminPrivileges = adminPrivileges;
            CustomerPermissions = customerPermissions.ToList();
            Blocked = blocked;
            FirstName = firstName;
            LastName = lastName;
        }

        public void SetAdminPrivileges(AdminPrivileges adminPrivileges)
        {
            AdminPrivileges = adminPrivileges;
        }

        public void SetCustomerPermissions(IEnumerable<CustomerPermissionGroup> customerPermissions)
        {
            CustomerPermissions = customerPermissions.ToList();
        }

        public void SetFirstName(FirstName firstName) => FirstName = firstName;
        public void SetLastName(LastName lastName) => LastName = lastName;
        public void SetEmail(Email email) => Email = email;

        public void SetBlocked(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Blocked:
                    Blocked = Blocked.Create(true);
                    break;
                case UserStatus.Active:
                    Blocked = Blocked.Create(false);
                    break;
                case UserStatus.BlockedAndActive:
                    Blocked = Blocked.Create(false);
                    break;
            }
        }
    
        public static User Create(UserId userId, UserName userName, Email email, PasswordHash passwordHash, SecurityStamp securityStamp, AdminPrivileges adminPrivileges, IEnumerable<CustomerPermissionGroup> customerPermissions, Blocked blocked, FirstName firstName, LastName lastName)
        {
            var user = new User(userId, userName, email, passwordHash, securityStamp, adminPrivileges, customerPermissions, blocked, firstName, lastName);
            user.AddDomainEvent(UserCreatedDomainEvent.Create(user.UserId, user.UserName, user.Email, user.AdminPrivileges, user.Blocked));
            return user;
        }

        public void SetUserDetails(UserName userName, UserRole userRole, UserStatus status,
            List<CustomerPermissionGroup> permissionGroupsToSet, FirstName firstName, LastName lastName, Email email)
        {
            SetUserName(userName);
            SetPrivileges(userRole);
            SetBlocked(status);
            SetCustomerPermissions(permissionGroupsToSet);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            AddDomainEvent(UserUpdatedDomainEvent.Create(UserId, UserName, Email,
                AdminPrivileges, Blocked, permissionGroupsToSet, FirstName, LastName));
        }

        private void SetPrivileges(UserRole userType)
        {
            switch (userType)
            {
                case UserRole.Admin:
                    AdminPrivileges = AdminPrivileges.Create(true);
                    break;

                case UserRole.User:
                    AdminPrivileges = AdminPrivileges.Create(false);
                    break;
            }
        }
        private void SetUserName(UserName userName)
        {
            UserName = userName;
        }

        public void UpdatePassword(PasswordHash passwordHash, SecurityStamp securityStamp)
        {
            PasswordHash = passwordHash;
            SecurityStamp = securityStamp;
        }
    }
}
