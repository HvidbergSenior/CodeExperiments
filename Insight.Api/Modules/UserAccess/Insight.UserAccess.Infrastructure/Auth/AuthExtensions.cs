using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Infrastructure.Auth
{
    public static class AuthExtensions
    {
        public static ApplicationUser ToApplicationUser(this User user)
        {
            return new ApplicationUser()
            {
                Id = user.UserId.Value.ToString(),
                UserName = user.UserName.Value,
                PasswordHash = user.PasswordHash.Value,
                Email = user.Email.Value,
                IsAdmin = user.AdminPrivileges.Value,
            };
        }
    }
}
