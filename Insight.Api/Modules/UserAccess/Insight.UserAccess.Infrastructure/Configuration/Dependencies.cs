using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Configuration;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.UserAccess.Application;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Insight.UserAccess.Infrastructure.Auth;
using Marten;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StoreOptions = Marten.StoreOptions;

namespace Insight.UserAccess.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddWithValidation<JWTOptions, JWTOptionsValidatior>("JWTKey");

            services.AddWithValidation<UserResetPasswordOptions, UserResetPasswordOptionsValidator>("UserResetPassword");

            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJWTOptions>();

            services.AddTransient<ITokenService, TokenService>();

            services.AddAuthorization();
            services.AddScoped<IExecutionContext, AuthenticatedExecutionContext>();
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            //TODO: 2 days ok?
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(2));

            services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IUserStore<ApplicationUser>, MartenUserStore>();
            services.AddTransient<IUserPasswordStore<ApplicationUser>, MartenUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, MartenRoleStore>();
            services.AddSingleton<IDefaultDataProvider, DefaultUserProvider>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();
            return services;
        }
    }

    public class ConfigureMarten : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema.For<User>().Duplicate(x => x.CustomerIds, "text[]");
        }
    }
}
