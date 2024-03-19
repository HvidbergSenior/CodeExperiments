using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class Blocked : ValueObject
    {
        public bool Value { get; private set; }

        private Blocked()
        {
            Value = false;
        }

        private Blocked(bool value)
        {
            Value = value;
        }

        public static Blocked Create(bool value)
        {
            return new Blocked(value);
        }

        public static Blocked None()
        {
            return new Blocked(false);
        }
    }
}