using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.BlockUser
{
    public sealed class BlockUserCommand : ICommand<ICommandResponse>
    {
        public UserName UserName { get; private set; }
        private BlockUserCommand(UserName userName)
        {
            UserName = userName;
        }

        public static BlockUserCommand Create(UserName userName)
        {
            return new BlockUserCommand(userName);
        }
    }

    internal class BlockUserCommandHandler : ICommandHandler<BlockUserCommand, ICommandResponse>
    {
        private readonly IExecutionContext executionContext;
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public BlockUserCommandHandler(IExecutionContext executionContext, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            this.executionContext = executionContext;
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ICommandResponse> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            if(request.UserName.Value == executionContext.UserName)
            {
                throw new BusinessException("I can't block myself as there is a risk of locking all admins out!");
            }
            var user = await userRepository.FindByUserName(request.UserName.Value, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }
            user.SetBlocked(UserStatus.Blocked);
            await userRepository.Update(user);

            // Invalidate all the old refresh tokens.
            refreshTokenRepository.DeleteAllRefreshTokensForUser(user.UserId);

            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class BlockUserCommandAuthorizer : IAuthorizer<BlockUserCommand>
    {
        IExecutionContext executionContext;
        public BlockUserCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(BlockUserCommand instance, CancellationToken cancellation)
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
