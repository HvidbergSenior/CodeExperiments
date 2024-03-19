using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
