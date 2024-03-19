using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Insight.UserAccess.Application.ResetPassword
{
    public sealed class ResetPasswordCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        public ResetPasswordToken ResetPasswordToken { get; private set; }
        public Password NewPassword { get; private set; }
        private ResetPasswordCommand(UserName userName, ResetPasswordToken resetPasswordToken, Password newPassword)
        {
            UserName = userName;
            ResetPasswordToken = resetPasswordToken;
            NewPassword = newPassword;
        }

        public static ResetPasswordCommand Create(UserName userName, ResetPasswordToken resetPasswordToken, Password newPassword)
        {
            return new ResetPasswordCommand(userName, resetPasswordToken, newPassword);
        }
    }

    internal class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ICommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UserResetPasswordOptions options;

        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender, IRefreshTokenRepository refreshTokenRepository, UserManager<ApplicationUser> userManager, IOptions<UserResetPasswordOptions> options)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
            this.refreshTokenRepository = refreshTokenRepository;
            this.userManager = userManager;
            this.options = options.Value;
        }
        public async Task<ICommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(request.UserName.Value);
            
            if(user == null)
            {
                throw new BusinessException("Cannot find the user to reset password to");
            }

            //Update password
            var result = await userManager.ResetPasswordAsync(user, request.ResetPasswordToken.Value, request.NewPassword.Value);

            if (!result.Succeeded)
            {
                throw new BusinessException("Could not change password. It could be the token is not correct or not valid anymore.");
            }
            // Invalidate all the old refresh tokens.
            refreshTokenRepository.DeleteAllRefreshTokensForUser(UserId.Create(Guid.Parse(user.Id)));

            if (user.UserName == null || user.Email == null)
            {
                throw new BusinessException("Cannot find the user and e-mail to reset password to");
            }

            await unitOfWork.Commit(cancellationToken);

            await emailSender.SendEmailAsync(ResetPasswordUser(options, user.UserName, user.Email));

            return EmptyCommandResponse.Default;
        }

        private static EmailMessage ResetPasswordUser(UserResetPasswordOptions options, string userName, string email)
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

    internal class ResetPasswordCommandAuthorizer : IAuthorizer<ResetPasswordCommand>
    {
        public ResetPasswordCommandAuthorizer()
        {

        }

        public Task<AuthorizationResult> Authorize(ResetPasswordCommand instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
            
        }
    }
}
