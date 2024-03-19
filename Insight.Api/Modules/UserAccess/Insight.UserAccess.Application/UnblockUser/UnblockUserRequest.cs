using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.UnblockUser
{
    public class UnblockUserRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }
    }
}
