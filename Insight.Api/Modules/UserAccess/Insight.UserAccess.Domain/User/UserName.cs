using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class UserName : ValueObject
    {
        public string Value { get; private set; }

        private UserName()
        {
            Value = string.Empty;
        }

        private UserName(string value)
        {
            Value = value;
        }

        public static UserName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }

            return new UserName(value);
        }

        public static UserName Empty()
        {
            return new UserName();
        }
    }
}
