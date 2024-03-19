using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.GetAccessToken
{
    public class AccessTokenRequest
    {
        [Required(ErrorMessage = "AccessToken is required")]
        public required string AccessToken { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "RefreshToken is required")]
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
