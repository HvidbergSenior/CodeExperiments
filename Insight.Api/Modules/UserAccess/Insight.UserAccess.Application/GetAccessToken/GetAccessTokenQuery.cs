using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Insight.UserAccess.Application.GetAccessToken
{
    public sealed class GetAccessTokenQuery : IQuery<GetAccessTokenResponse>
    {
        private GetAccessTokenQuery(RefreshToken refreshToken, AccessToken accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }

        public RefreshToken RefreshToken { get; private set; }
        public AccessToken AccessToken { get; private set; }

        public static GetAccessTokenQuery Create(RefreshToken refreshToken, AccessToken accessToken)
        {
            return new GetAccessTokenQuery(refreshToken, accessToken);
        }
    }

    internal class GetAccessTokenQueryHandler : IQueryHandler<GetAccessTokenQuery, GetAccessTokenResponse>
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public GetAccessTokenQueryHandler(ITokenService tokenService, UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.refreshTokenRepository = refreshTokenRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<GetAccessTokenResponse> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken.Value);

            var username = UserName.Create(principal.Identity!.Name!); //this is mapped to the Name claim by default
            var user = await userManager.FindByNameAsync(username.Value);
            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }

            var userId = UserId.Create(Guid.Parse(user.Id));
            var validRefreshTokens = await refreshTokenRepository.GetRefreshTokensAsync(userId, cancellationToken);
            var token = validRefreshTokens.FirstOrDefault(c => c.Value == request.RefreshToken.Value);

            if (token == null || token.ExpirationDate <= DateTimeOffset.UtcNow)
            {
                if (token != null)
                {
                    var tokenId = RefreshTokenId.Create(token.Id);
                    refreshTokenRepository.DeleteRefreshToken(tokenId);
                    await unitOfWork.Commit(cancellationToken);
                }
                throw new BusinessException("Refreshtoken not found or expired");
            }

            var newAccessToken = AccessToken.Create(tokenService.GenerateAccessToken(principal.Claims));


            return new GetAccessTokenResponse(newAccessToken);
        }
    }

    internal class GetAccessTokenQueryAuthorizer : IAuthorizer<GetAccessTokenQuery>
    {
        public Task<AuthorizationResult> Authorize(GetAccessTokenQuery instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
