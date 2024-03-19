using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class LastName : ValueObject
    {
        public string Value { get; private set; }

        private LastName()
        {
            Value = string.Empty;
        }

        private LastName(string value)
        {
            Value = value;
        }

        public static LastName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty", nameof(value));
            }

            return new LastName(value);
        }

        public static LastName Empty()
        {
            return new LastName();
        }
    }
}
