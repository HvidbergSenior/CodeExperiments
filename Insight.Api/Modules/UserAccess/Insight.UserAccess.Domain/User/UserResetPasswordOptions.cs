using FluentValidation;

namespace Insight.UserAccess.Domain.User;

public class UserResetPasswordOptions
{
    public string ResetPasswordFrontendStartWithUserUrl { get; set; } = string.Empty;
    public string ResetPasswordFrontendTokenEndWithTokenUrl { get; set; } = string.Empty;
    public string ResetPasswordEmailFrom { get; set; } = string.Empty;
}
public class UserResetPasswordOptionsValidator : AbstractValidator<UserResetPasswordOptions>
{
    public UserResetPasswordOptionsValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(t => t.ResetPasswordFrontendStartWithUserUrl).NotNull().NotEmpty();
        RuleFor(t => t.ResetPasswordFrontendTokenEndWithTokenUrl).NotNull().NotEmpty();
        RuleFor(t => t.ResetPasswordEmailFrom).NotNull().NotEmpty();
    }
}