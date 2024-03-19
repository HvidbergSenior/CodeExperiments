using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class AccountId : ValueObject
    {
        public string Value { get; private set; }

        private AccountId()
        {
            Value = string.Empty;
        }

        private AccountId(string value)
        {
            Value = value;
        }

        public static AccountId Create(string value)
        {
            return new AccountId(value);
        }

        public static AccountId Empty()
        {
            return new AccountId();
        }
    }
}