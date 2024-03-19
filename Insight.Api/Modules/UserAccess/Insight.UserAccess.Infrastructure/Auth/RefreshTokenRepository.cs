using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Marten;

namespace Insight.UserAccess.Infrastructure.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDocumentSession documentSession;

        public RefreshTokenRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void CreateRefreshToken(RefreshTokenContainer refreshToken)
        {
            documentSession.Store(refreshToken);
        }

        public void DeleteAllRefreshTokensForUser(UserId userId)
        {
            documentSession.DeleteWhere<RefreshTokenContainer>(c => c.UserId.Value == userId.Value);
        }

        public void DeleteRefreshToken(RefreshTokenId refreshTokenId)
        {
            documentSession.Delete<RefreshTokenContainer>(refreshTokenId.Value);
        }

        public Task<IReadOnlyList<RefreshTokenContainer>> GetRefreshTokensAsync(UserId userId, CancellationToken cancellationToken)
        {
            return documentSession.Query<RefreshTokenContainer>().Where(c => c.UserId.Value == userId.Value).ToListAsync(cancellationToken);
        }
    }
}
