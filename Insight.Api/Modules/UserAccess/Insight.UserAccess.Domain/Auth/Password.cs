using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.Auth
{
    public sealed class Password : ValueObject
    {
        public string Value { get; private set; }

        private Password()
        {
            Value = string.Empty;
        }

        private Password(string value)
        {
            Value = value;
        }

        public static Password Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }

            return new Password(value);
        }

        public static Password Empty()
        {
            return new Password();
        }
    }
}
