using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; private set; }

        private UserId()
        {
            Value = Guid.Empty;
        }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Guid is empty", nameof(value));
            }

            return new UserId(value);
        }

        public static UserId Empty()
        {
            return new UserId(Guid.Empty);
        }
    }
}
