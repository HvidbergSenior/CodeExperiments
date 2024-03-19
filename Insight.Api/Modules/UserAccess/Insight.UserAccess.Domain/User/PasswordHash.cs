using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get; private set; }

        private PasswordHash()
        {
            Value = String.Empty;
        }

        private PasswordHash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }
            Value = value;
        }

        public static PasswordHash Create(string email)
        {
            return new PasswordHash(email);
        }

        public static PasswordHash Empty()
        {
            return new PasswordHash();
        }
    }
}
