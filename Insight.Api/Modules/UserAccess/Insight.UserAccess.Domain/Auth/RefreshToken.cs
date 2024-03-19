using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.Auth
{
    public sealed class RefreshToken : ValueObject
    {
        public string Value { get; private set; }

        private RefreshToken()
        {
            Value = String.Empty;
        }

        private RefreshToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }
            Value = value;
        }

        public static RefreshToken Create(string refreshToken)
        {
            return new RefreshToken(refreshToken);
        }

        public static RefreshToken Empty()
        {
            return new RefreshToken();
        }
    }
}
