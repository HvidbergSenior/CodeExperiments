using Insight.UserAccess.Domain.Auth;

namespace Insight.UserAccess.Application.GetAccessToken
{
    public class GetAccessTokenResponse
    {
        public GetAccessTokenResponse(AccessToken accessToken)
        {
            AccessToken = accessToken;
        }

        public AccessToken AccessToken { get; private set; }
    }
}
