using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Insight.UserAccess.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JWTOptions jWTOptions;

        public TokenService(IOptions<JWTOptions> jWTOptions)
        {
            this.jWTOptions = jWTOptions.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTOptions.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jWTOptions.ValidIssuer,
                Audience = jWTOptions.ValidAudience,
                Expires = DateTime.UtcNow.AddHours(jWTOptions.AccessTokenExpiryTimeInHours),                
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = jWTOptions.ValidAudience,
                ValidateAudience = true,
                ValidIssuer = jWTOptions.ValidIssuer,
                ValidateIssuer = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTOptions.Secret)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, //here we are saying that we don't care about the token's expiration date
            };
            IdentityModelEventSource.ShowPII = true;

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public int GetRefreshTokenExpirationTimeInHours()
        {
            return jWTOptions.RefreshTokenExpiryTimeInHours;
        }
    }
}
