using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Insight.UserAccess.Infrastructure.Auth
{
    public class MartenUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        private readonly IUserRepository userRepository;
        
        public MartenUserStore(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;            
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var couldParse = Enum.TryParse(roleName, ignoreCase: true, out UserRole role);

            if (!couldParse)
            {
                throw new NotSupportedException("Invalid role");
            }           

            switch (role)
            {
                case UserRole.Admin:
                    {
                        user.SetAdmin(true);
                    }
                    break;
            
                case UserRole.User:
                    {
                        user.SetAdmin(false);
                    }
                    break;
            }
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user.UserName == null)
            {
                throw new ArgumentNullException(nameof(user),"Username is null");
            }
            if (user.Email == null)
            {
                throw new ArgumentNullException(nameof(user), "Email is null");
            }
            if (user.FirstName == null)
            {
                throw new ArgumentNullException(nameof(user), "FirstName is null");
            }  
            if (user.LastName == null)
            {
                throw new ArgumentNullException(nameof(user), "LastName is null");
            }
            if (user.PasswordHash == null)
            {
                throw new ArgumentNullException(nameof(user), "PasswordHash is null");
            }

            if (user.SecurityStamp == null)
            {
                throw new ArgumentNullException(nameof(user), "SecurityStamp is null");
            }

            var userId = UserId.Create(Guid.Parse(user.Id));
            var name = UserName.Create(user.UserName);
            var mail = Email.Create(user.Email);
            var passwordHash = PasswordHash.Create(user.PasswordHash);
            var securityStamp = SecurityStamp.Create(user.SecurityStamp);
            var adminPrivileges = AdminPrivileges.Create(user.IsAdmin);
            var customerPermissions = user.CustomerPermissionGroups;
            var blocked = Blocked.Create(false);
            var firstName = FirstName.Create(user.FirstName);
            var lastName = LastName.Create(user.LastName);

            var userEntity = User.Create(userId, name, mail, passwordHash, securityStamp, adminPrivileges, customerPermissions, blocked, firstName, lastName);

            await userRepository.Add(userEntity, cancellationToken);
            await userRepository.SaveChanges(cancellationToken);
            
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await userRepository.DeleteById(Guid.Parse(user.Id), cancellationToken);
            
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindById(userId, cancellationToken);

            return user?.ToApplicationUser();
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByUserName(normalizedUserName, cancellationToken);

            return user?.ToApplicationUser();
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        { 
            var roles = new List<string>();

            roles.Add(user.IsAdmin ? "Admin" : "User");

            return Task.FromResult<IList<string>>(roles);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {  
            var couldParse = Enum.TryParse(roleName,ignoreCase: true, out UserRole role);

            if (!couldParse)
            {
                throw new NotSupportedException("Invalid role");
            }

            switch (role)
            {
                case UserRole.Admin:
                    return Task.FromResult(user.IsAdmin);
                case UserRole.User:
                    return Task.FromResult(!user.IsAdmin);
                default:
                    throw new NotImplementedException($"Role {roleName} not implemented");
            }
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            //This is only intended to update passwords so I am only implementing only security related updates here.
            //It is only used internally by Microsoft.AspNetCore.Identity.UserManager
            var userFromRepository = await userRepository.FindById(Guid.Parse(user.Id));
            if (userFromRepository == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "ErrorFindingUser",
                    Description = $"Cannot find user with id {user.Id}"
                });
            }
            if (user.PasswordHash == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "ErrorPasswordHash",
                    Description = $"PasswordHash is null for {user.Id}"
                });
            }
            if (user.SecurityStamp == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "ErrorSecurityStamp",
                    Description = $"SecurityStamp is null"
                });
            }

            userFromRepository.UpdatePassword(PasswordHash.Create(user.PasswordHash), SecurityStamp.Create(user.SecurityStamp));

            await userRepository.Update(userFromRepository, cancellationToken);
            await userRepository.SaveChanges(cancellationToken);

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userFromRepository = await userRepository.FindById(Guid.Parse(user.Id), cancellationToken);

            return userFromRepository?.Email.Value;
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            //Implement me if you need me. UserManager code may try to use it even though it is not needed so not throwing an exception here
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            //Implement me if you need me. UserManager code may try to use it even though it is not needed so not throwing an exception here
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByEmail(normalizedEmail, cancellationToken);

            return user?.ToApplicationUser();
        }

        public async Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await GetEmailAsync(user, cancellationToken);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            //Implement me if you need me. UserManager code may try to use it even though it is not needed so not throwing an exception here
            return Task.CompletedTask;
        }
    }
}
