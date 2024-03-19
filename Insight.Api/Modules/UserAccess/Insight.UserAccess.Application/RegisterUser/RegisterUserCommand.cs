using System.Web;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.UserAccess;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Insight.UserAccess.Application.RegisterUser
{
    public sealed class RegisterUserCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Password ConfirmPassword { get; private set; }
        public UserRole Role { get; private set; }
        public IEnumerable<CustomerPermissionGroup> CustomerPermissionGroups { get; private set; }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public UserStatus Status { get; private set; }
        private RegisterUserCommand(UserName userName, Email email, Password password, Password confirmPassword, UserRole role, IEnumerable<CustomerPermissionGroup> customerPermissionGroups, FirstName firstName, LastName lastName, UserStatus status)
        {
            UserName = userName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Role = role;
            CustomerPermissionGroups = customerPermissionGroups;
            FirstName = firstName;
            LastName = lastName;
            Status = status;
        }

        public static RegisterUserCommand Create(UserName userName, Email email, Password password, Password confirmPassword, UserRole userRole, IEnumerable<CustomerPermissionGroup> customerPermissionGroups, FirstName firstName, LastName lastName, UserStatus status)
        {
            if (!password.Value.Equals(confirmPassword.Value, StringComparison.Ordinal))
            {
                throw new BusinessException("Passwords must match");
            }

            return new RegisterUserCommand(userName, email, password, confirmPassword, userRole, customerPermissionGroups, firstName, lastName, status);
        }
    }

    internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, ICommandResponse>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IExecutionContext executionContext;
        private readonly IEmailSender emailSender;
        private readonly UserResetPasswordOptions options;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IExecutionContext executionContext, IEmailSender emailSender, IOptions<UserResetPasswordOptions> options)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.executionContext = executionContext;
            this.emailSender = emailSender;
            this.options = options.Value;
        }

        public async Task<ICommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!(await executionContext.GetAdminPrivileges()))
            {
                var customerPermissionsToSetNewUser = request.CustomerPermissionGroups;
                var myCustomerPermissions =
                    await executionContext.GetCustomersPermissionsAsync(true, cancellationToken);
                var (permissionsCanBeSet, errorMessage) =
                    UserAccessHelper.PermissionsCanBeSet(myCustomerPermissions, customerPermissionsToSetNewUser);
                if (!permissionsCanBeSet)
                {
                    throw new BusinessException(
                        $"Logged in user cannot set permissions on new registered user. Error message from check is {errorMessage}");
                }
            }

            var userExistsUserName = await userManager.FindByNameAsync(request.UserName.Value);
            var userExistsEmail = await userManager.FindByEmailAsync(request.Email.Value);
            if (userExistsUserName != null || userExistsEmail != null)
            {
                throw new AlreadyExistingException("User already exists");
            }

            ApplicationUser user = new()
            {
                Email = request.Email.Value,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.UserName.Value,
                CustomerPermissionGroups = request.CustomerPermissionGroups,
                FirstName = request.FirstName.Value,
                LastName = request.LastName.Value
            };

            await AddUserToRoleAsync(request, user);

            var createUserResult = await userManager.CreateAsync(user, request.Password.Value);

            if (!createUserResult.Succeeded)
                throw new BusinessException("User creation failed!");

            var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);

            if (!(await emailSender.SendEmailAsync(CreateMessageNewUser(options, user.UserName, user.Email, resetPasswordToken))))
            {
                throw new Exception("Error sending e-mail when registering user. Aborting register process");
            };

            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static EmailMessage CreateMessageNewUser(UserResetPasswordOptions options, string userName, string email, string token)
        {
            var link = options.ResetPasswordFrontendStartWithUserUrl + HttpUtility.UrlEncode(userName) + options.ResetPasswordFrontendTokenEndWithTokenUrl + HttpUtility.UrlEncode(token);
            var message = new EmailMessage(
                options.ResetPasswordEmailFrom,
                new List<string>() { email },
                "Welcome to Biofuel Express - Please create a password",
                $"<p>Welcome to Biofuel Express {userName}!</p>" +
                          $"<p>It is important that you set up a new password using the following link (copy and paste it if you can't open it directly):</p>" +
                          $"<ul><li><a href=\"{link}\">{link}</a></li></ul>"
            );
            return message;
        }

        private async Task AddUserToRoleAsync(RegisterUserCommand request, ApplicationUser user)
        {
            await userManager.AddToRoleAsync(user, request.Role.ToString());
        }
    }

    internal class RegisterUserCommandAuthorizer : IAuthorizer<RegisterUserCommand>
    {
        private readonly IExecutionContext executionContext;

        public RegisterUserCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(RegisterUserCommand instance, CancellationToken cancellationToken)
        {
            var myCustomerPermissions =
                await executionContext.GetCustomersPermissionsAsync(true, cancellationToken);
            
            if (await executionContext.GetAdminPrivileges(cancellationToken))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
