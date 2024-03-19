using System.Web;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using JasperFx.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Insight.UserAccess.Application.ForgotPassword
{
    public sealed class ForgotPasswordCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        private ForgotPasswordCommand(UserName userName, Email email)
        {
            UserName = userName;
            Email = email;
        }

        public static ForgotPasswordCommand Create(UserName userName, Email email)
        {
            return new ForgotPasswordCommand(userName, email);
        }
    }

    internal class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ICommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UserResetPasswordOptions options;

        public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IOptions<UserResetPasswordOptions> options)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.options = options.Value;
        }
        public async Task<ICommandResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = null;
            if(!request.UserName.Value.IsEmpty())
            {
                user = await userManager.FindByNameAsync(request.UserName.Value);
            }
            else if(!request.Email.Value.IsEmpty())
            {
                user = await userManager.FindByEmailAsync(request.Email.Value);
            }
            
            if(user == null)
            {
                throw new BusinessException("Cannot find the user or e-mail to reset password to");
            }

            var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);

            if (user.UserName == null || user.Email == null)
            {
                throw new BusinessException("Cannot find the user and e-mail to reset password to");
            }

            if (!(await emailSender.SendEmailAsync(CreateMessageForgotPassword(options, user.UserName, user.Email,
                    resetPasswordToken))))
            {
                throw new Exception("Error sending e-mail when requesting a forgot password. Aborting forgot password process");
            }

            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static EmailMessage CreateMessageForgotPassword(UserResetPasswordOptions options, string userName, string email, string token)
        {
            var link = options.ResetPasswordFrontendStartWithUserUrl + HttpUtility.UrlEncode(userName) + options.ResetPasswordFrontendTokenEndWithTokenUrl + HttpUtility.UrlEncode(token);
            var message = new EmailMessage(
                options.ResetPasswordEmailFrom,
                new List<string>() { email },
                "Password reset for Biofuel Express",
                $"<p>Hello {userName}!</p>" +
                          $"<p>If you have forgotten your password you can reset it to a new one using the link below (copy and paste it if you can't open it directly).</p>" +
                          $"<ul><li><a href=\"{link}\">{link}</a></li></ul>" + 
                          "<p>If you have not requested this you can ignore this e-mail. Someone may have typed your username or password by mistake.</p>"
            );
            return message;
        }
    }

    internal class ForgotPasswordCommandAuthorizer : IAuthorizer<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandAuthorizer()
        {

        }

        public Task<AuthorizationResult> Authorize(ForgotPasswordCommand instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
            
        }
    }
}
