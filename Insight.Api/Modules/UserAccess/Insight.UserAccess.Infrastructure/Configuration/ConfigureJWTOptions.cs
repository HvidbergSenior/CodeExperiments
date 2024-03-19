using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Insight.UserAccess.Infrastructure.Configuration
{
    public class ConfigureJWTOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public ConfigureJWTOptions(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var jwtOptions = provider.GetRequiredService<IOptions<JWTOptions>>();

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Value.ValidAudience,
                    ValidIssuer = jwtOptions.Value.ValidIssuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret))
                };
            }
        }
    }
}
