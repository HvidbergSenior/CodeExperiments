using Insight.BuildingBlocks.Domain;
using Microsoft.AspNetCore.Identity;

namespace Insight.UserAccess.Domain.Auth
{
    public sealed class ApplicationUser : IdentityUser
    {
        public void SetAdmin(bool admin)
        {
            IsAdmin = admin;
        }
        public bool IsAdmin { get; set; }
        public IEnumerable<CustomerPermissionGroup> CustomerPermissionGroups { get; set; } = new List<CustomerPermissionGroup>();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
