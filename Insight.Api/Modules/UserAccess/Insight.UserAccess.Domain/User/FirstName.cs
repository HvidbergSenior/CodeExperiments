using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class FirstName : ValueObject
    {
        public string Value { get; private set; }

        private FirstName()
        {
            Value = string.Empty;
        }

        private FirstName(string value)
        {
            Value = value;
        }

        public static FirstName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }

            return new FirstName(value);
        }

        public static FirstName Empty()
        {
            return new FirstName();
        }
    }
}
