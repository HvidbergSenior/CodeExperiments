using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.BlockUser
{
    public class BlockUserRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }
    }
}
