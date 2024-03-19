using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class AccountCustomerId : ValueObject
    {
        public Guid Value { get; private set; }

        private AccountCustomerId()
        {
            Value = Guid.Empty;
        }

        private AccountCustomerId(Guid value)
        {
            Value = value;
        }

        public static AccountCustomerId Create(Guid value)
        {
            return new AccountCustomerId(value);
        }

        public static AccountCustomerId Empty()
        {
            return new AccountCustomerId();
        }
    }
}
