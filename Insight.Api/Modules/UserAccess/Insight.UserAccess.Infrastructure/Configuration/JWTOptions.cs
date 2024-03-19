using FluentValidation;

namespace Insight.UserAccess.Infrastructure.Configuration
{
    public class JWTOptions
    {
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int AccessTokenExpiryTimeInHours { get; set; }
        public int RefreshTokenExpiryTimeInHours { get; set; }
    }
    public class JWTOptionsValidatior : AbstractValidator<JWTOptions>
    {
        public JWTOptionsValidatior()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(t => t.ValidAudience).NotEmpty();
            RuleFor(t => t.ValidIssuer).NotEmpty();
            RuleFor(t => t.Secret).NotEmpty();
            RuleFor(t => t.AccessTokenExpiryTimeInHours).NotEmpty();
            RuleFor(t => t.RefreshTokenExpiryTimeInHours).NotEmpty();

        }
    }

}
