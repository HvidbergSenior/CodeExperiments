using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Domain.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<IReadOnlyList<RefreshTokenContainer>> GetRefreshTokensAsync(UserId userId, CancellationToken cancellationToken);
        void CreateRefreshToken(RefreshTokenContainer refreshToken);
        void DeleteRefreshToken(RefreshTokenId refreshTokenId);
        void DeleteAllRefreshTokensForUser(UserId userId);
    }
}
