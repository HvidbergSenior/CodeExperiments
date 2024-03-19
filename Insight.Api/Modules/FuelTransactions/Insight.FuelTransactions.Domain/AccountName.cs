using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class AccountName : ValueObject
    {
        public string Value { get; private set; }

        private AccountName()
        {
            Value = string.Empty;
        }

        private AccountName(string value)
        {
            Value = value;
        }

        public static AccountName Create(string value)
        {
            return new AccountName(value);
        }

        public static AccountName Empty()
        {
            return new AccountName();
        }
    }
}
