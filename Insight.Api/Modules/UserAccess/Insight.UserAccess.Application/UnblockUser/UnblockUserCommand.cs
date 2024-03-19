using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.UnblockUser
{
    public sealed class UnblockUserCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        private UnblockUserCommand(UserName userName)
        {
            UserName = userName;
        }

        public static UnblockUserCommand Create(UserName userName)
        {
            return new UnblockUserCommand(userName);
        }
    }

    internal class UnblockUserCommandHandler : ICommandHandler<UnblockUserCommand, ICommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public UnblockUserCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ICommandResponse> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByUserName(request.UserName.Value, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }
            user.SetBlocked(UserStatus.Active);
            await userRepository.Update(user);

            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class UnblockUserCommandAuthorizer : IAuthorizer<UnblockUserCommand>
    {
        IExecutionContext executionContext;
        public UnblockUserCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(UnblockUserCommand instance, CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            else
            {
                return AuthorizationResult.Fail();
            }
            
        }
    }
}
