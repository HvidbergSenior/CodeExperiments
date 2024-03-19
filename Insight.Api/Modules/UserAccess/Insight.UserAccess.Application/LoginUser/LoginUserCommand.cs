using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Insight.UserAccess.Application.LoginUser
{
    public sealed class LoginUserCommand : ICommand<LoginUserCommandResponse>
    {
        public UserName UserName { get; private set; }
        public Password Password { get; private set; }

        private LoginUserCommand(UserName userName, Password password)
        {
            UserName = userName;
            Password = password;
        }

        public static LoginUserCommand Create(UserName userName, Password password)
        {
            return new LoginUserCommand(userName, password);
        }
    }

    internal class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.refreshTokenRepository = refreshTokenRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.UserName.Value);
            var userDomain = await userRepository.FindByUserName(request.UserName.Value, cancellationToken);
            if (user == null || userDomain == null)
                throw new NotFoundException("User not found");
            if (userDomain.Blocked.Value)
                throw new Exception("User is blocked");
            if (!await userManager.CheckPasswordAsync(user, request.Password.Value))
                throw new Exception("Incorrect password");

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, request.UserName.Value),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            if(user.IsAdmin)
            {
                Enum.GetValues<CustomerPermission>().ToList().ForEach(p => authClaims.Add(new Claim("access", p.ToString())));
            }
            else
            {
                var myPermissions = userDomain.CustomerPermissions.SelectMany(p => p.Permissions).Select(p => p).ToList();

                // It's a minimum requirement that a user has FuelConsumption permissions.
                if (!myPermissions.Contains(BuildingBlocks.Domain.CustomerPermission.FuelConsumption))
                {
                    myPermissions.Add(BuildingBlocks.Domain.CustomerPermission.FuelConsumption);
                }

                foreach (var permission in myPermissions.Distinct())
                {
                    authClaims.Add(new Claim("access", permission.ToString()));
                }
            }            

            var accessToken = AccessToken.Create(tokenService.GenerateAccessToken(authClaims));
            var refreshToken = RefreshToken.Create(tokenService.GenerateRefreshToken());

            var refreshTokenLifetime = tokenService.GetRefreshTokenExpirationTimeInHours();

            var tokenToPersist = new RefreshTokenContainer(UserId.Create(Guid.Parse(user.Id)), refreshToken.Value, DateTimeOffset.UtcNow.AddHours(refreshTokenLifetime));

            refreshTokenRepository.CreateRefreshToken(tokenToPersist);

            await unitOfWork.Commit(cancellationToken);

            return new LoginUserCommandResponse(refreshToken, accessToken);
        }
    }

    internal class LoginUserCommandAuthorizer : IAuthorizer<LoginUserCommand>
    {
        public Task<AuthorizationResult> Authorize(LoginUserCommand instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
