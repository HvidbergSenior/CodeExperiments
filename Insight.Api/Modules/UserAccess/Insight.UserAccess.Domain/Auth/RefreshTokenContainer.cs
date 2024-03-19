using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Domain.Auth
{
    public class RefreshTokenContainer
    {
        public Guid Id { get; private set; }
        public UserId UserId { get; private set; }
        public string Value { get; private set; }
        public DateTimeOffset ExpirationDate { get; private set; }

        public RefreshTokenContainer(UserId userId, string token, DateTimeOffset expirationDate)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Value = token;
            ExpirationDate = expirationDate;
        }
    }
}
