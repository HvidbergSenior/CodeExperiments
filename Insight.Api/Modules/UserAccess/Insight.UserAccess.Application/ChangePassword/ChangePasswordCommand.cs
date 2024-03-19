using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Insight.UserAccess.Application.ChangePassword
{
    public sealed class ChangePasswordCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        public Password CurrentPassword { get; private set; }
        public Password NewPassword { get; private set; }
        public Password ConfirmPassword { get; private set; }
        private ChangePasswordCommand(UserName userName, Password currentPassword, Password newPassword, Password confirmPassword)
        {
            UserName = userName;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public static ChangePasswordCommand Create(UserName userName, Password currentPassword, Password newPassword, Password confirmPassword)
        {
            if (!newPassword.Value.Equals(confirmPassword.Value, StringComparison.Ordinal))
            {
                throw new ArgumentException("Passwords do not match");
            }

            return new ChangePasswordCommand(userName, currentPassword, newPassword, confirmPassword);
        }
    }

    internal class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ICommandResponse>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        private readonly UserResetPasswordOptions options;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IEmailSender emailSender, IOptions<UserResetPasswordOptions> options)
        {
            this.userManager = userManager;
            this.refreshTokenRepository = refreshTokenRepository;
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
            this.options = options.Value;
        }
        public async Task<ICommandResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.UserName.Value);
            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword.Value, request.NewPassword.Value);

            if (!result.Succeeded)
            {
                throw new BusinessException("Password mismatch");
            }

            var userId = UserId.Create(Guid.Parse(user.Id));

            // Invalidate all the old refresh tokens.
            refreshTokenRepository.DeleteAllRefreshTokensForUser(userId);

            await unitOfWork.Commit(cancellationToken);

            if (user.Email != null)
            {
                await emailSender.SendEmailAsync(ChangePasswordUser(options, user.UserName ?? "", user.Email));
            }

            return EmptyCommandResponse.Default;
        }

        private static EmailMessage ChangePasswordUser(UserResetPasswordOptions options, string userName, string email)
        {
            var message = new EmailMessage(
                options.ResetPasswordEmailFrom,
                new List<string>() { email },
                "Your password has been changed for Biofuel Express",
                $"<p>Hello {userName}!</p>" +
                $"<p>Your password has successfully been changed trough the forget password you previously received.</p><p>Note: If you did not do this your e-mail may have been compromised.</p>"
            );
            return message;
        }
    }

    internal class ChangePasswordCommandAuthorizer : IAuthorizer<ChangePasswordCommand>
    {
        public Task<AuthorizationResult> Authorize(ChangePasswordCommand instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
