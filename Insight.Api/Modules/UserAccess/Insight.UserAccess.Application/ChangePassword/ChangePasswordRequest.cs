using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.ChangePassword
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation password is required")]
        public required string ConfirmPassword { get; set; }
    }
}
