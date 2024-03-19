using System.Security.Claims;

namespace Insight.UserAccess.Domain.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        int GetRefreshTokenExpirationTimeInHours();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
