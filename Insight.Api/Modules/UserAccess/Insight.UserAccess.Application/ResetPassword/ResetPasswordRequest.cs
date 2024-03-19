using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.ResetPassword
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Username of the user to change password to is needed")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Token to reset password is required")]
        public required string Token { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public required string NewPassword { get; set; }
    }
}
