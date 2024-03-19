using System.ComponentModel.DataAnnotations;
namespace Insight.UserAccess.Application.GetAccessToken
{
    public class AuthenticatedResponse
    {
        public AuthenticatedResponse(string refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }

        [Required]
        public string RefreshToken { get; }
        [Required]
        public string AccessToken { get; }
    }
}
