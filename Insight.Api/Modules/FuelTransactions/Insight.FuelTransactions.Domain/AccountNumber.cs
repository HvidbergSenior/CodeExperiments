using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class AccountNumber : ValueObject
    {
        public string Value { get; private set; }

        private AccountNumber()
        {
            Value = string.Empty;
        }

        private AccountNumber(string value)
        {
            Value = value;
        }

        public static AccountNumber Create(string value)
        {
            return new AccountNumber(value);
        }

        public static AccountNumber Empty()
        {
            return new AccountNumber();
        }
    }
}
