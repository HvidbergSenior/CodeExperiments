using Insight.UserAccess.Domain.Auth;

namespace Insight.UserAccess.Application.LoginUser
{
    public class LoginUserCommandResponse
    {
        public LoginUserCommandResponse(RefreshToken refreshToken, AccessToken accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }

        public RefreshToken RefreshToken { get; private set; }
        public AccessToken AccessToken { get; private set; }
    }
}
