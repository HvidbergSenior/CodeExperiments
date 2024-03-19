using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.UserAccess;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.UpdateUser
{
    public sealed class UpdateUserCommand : ICommand<UpdateUserCommandResponse>
    {
        internal UserUpdateParameters UserUpdateParameters { get; }
        public IEnumerable<CustomerPermissionGroup> CustomerPermissionGroups { get; private set; }


        private UpdateUserCommand(UserUpdateParameters userUpdateParameters, IEnumerable<CustomerPermissionGroup> customerPermissionGroups)
        {
            UserUpdateParameters = userUpdateParameters;
            CustomerPermissionGroups = customerPermissionGroups;
        }

        public static UpdateUserCommand Create(UserUpdateParameters userUpdateParameters, IEnumerable<CustomerPermissionGroup> customerPermissionGroups)
        {
            return new UpdateUserCommand(userUpdateParameters, customerPermissionGroups);
        }
    }

    internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand,
        UpdateUserCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IExecutionContext executionContext;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IExecutionContext executionContext)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.executionContext = executionContext;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var customerPermissionsToSetEditedUser = request.CustomerPermissionGroups;
            var userToUpdate = await userRepository.FindById(request.UserUpdateParameters.UserId.Value);
            if (userToUpdate == null)
            {
                throw new BusinessException(
                    $"Cant update user with id {request.UserUpdateParameters.UserId.Value} as I cant find it.");
            }
            var allCustomerPermissions = userToUpdate.CustomerPermissions;
            IEnumerable<CustomerPermissionGroup> allNewPermissions;
            var loggedInUserPermissions = await executionContext.GetCustomersPermissionsAsync(true, cancellationToken);
            if (!await executionContext.GetAdminPrivileges())
            {
                var (permissionsCanBeSet, errorMessage) = UserAccessHelper.PermissionsCanBeSet(loggedInUserPermissions, customerPermissionsToSetEditedUser);
                
                if (!permissionsCanBeSet)
                {
                    throw new BusinessException($"Logged in user cannot set permissions on user to be updated. Error message from check is {errorMessage}");
                }

                var permissionsICantTouch = allCustomerPermissions
                    .Where(p => !loggedInUserPermissions.Any(lp => lp.CustomerId.Value == p.CustomerId.Value && lp.Permissions.Contains(CustomerPermission.Admin)));
                allNewPermissions = permissionsICantTouch.Concat(customerPermissionsToSetEditedUser);
            }
            else
            {
                allNewPermissions = customerPermissionsToSetEditedUser;
            }

            var user = await userRepository.FindById(request.UserUpdateParameters.UserId.Value);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var newUserNameUser = await userRepository.FindByUserName(request.UserUpdateParameters.UserName.Value, cancellationToken);
            var newEmailUser = await userRepository.FindByEmail(request.UserUpdateParameters.Email.Value, cancellationToken);
            if (newUserNameUser != null && newUserNameUser.UserId.Value != request.UserUpdateParameters.UserId.Value)
            {
                throw new BusinessException("I can't update to that username as it is already used by another user.");
            }
            if (newEmailUser != null && newEmailUser.Email.Value != request.UserUpdateParameters.Email.Value)
            {
                throw new BusinessException("I can't update to that email as it is already used by another user.");
            }

            user.SetUserDetails(request.UserUpdateParameters.UserName, request.UserUpdateParameters.UserType, request.UserUpdateParameters.Status, allNewPermissions.ToList(), request.UserUpdateParameters.FirstName, request.UserUpdateParameters.LastName, request.UserUpdateParameters.Email);

            await userRepository.Update(user, cancellationToken);
            await userRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return MapToUserUpdateResponses(user);
        }

        private static UpdateUserCommandResponse MapToUserUpdateResponses(
            User user)
        {
            return new UpdateUserCommandResponse(
                new UpdateUserResponse(user.UserName.Value,
                user.UserId.Value));
        }
    }

    internal class UpdateUserCommandAuthorizer : IAuthorizer<UpdateUserCommand>
    {
        private readonly IExecutionContext executionContext;

        public UpdateUserCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UpdateUserCommand command,
            CancellationToken cancellation)
        {
            var hasAdminPrivileges = await executionContext.GetAdminPrivileges(cancellation);
            var hasCustomerAdminPermission = (await executionContext.GetCustomersPermissionsAsync(cancellationToken: cancellation)).Any(p => p.Permissions.Contains(CustomerPermission.Admin));
            if (hasAdminPrivileges || hasCustomerAdminPermission)
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
    
}