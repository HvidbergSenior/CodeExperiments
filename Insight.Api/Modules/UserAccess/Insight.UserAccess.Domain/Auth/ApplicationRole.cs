using Microsoft.AspNetCore.Identity;

namespace Insight.UserAccess.Domain.Auth
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
