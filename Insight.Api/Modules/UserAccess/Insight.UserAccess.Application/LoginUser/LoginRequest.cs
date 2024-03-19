using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.LoginUser
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}