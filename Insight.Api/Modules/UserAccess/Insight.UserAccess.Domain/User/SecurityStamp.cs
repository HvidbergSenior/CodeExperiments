using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class SecurityStamp : ValueObject
    {
        public string Value { get; private set; }

        private SecurityStamp()
        {
            Value = String.Empty;
        }

        private SecurityStamp(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }
            Value = value;
        }

        public static SecurityStamp Create(string email)
        {
            return new SecurityStamp(email);
        }

        public static SecurityStamp Empty()
        {
            return new SecurityStamp();
        }
    }
}
