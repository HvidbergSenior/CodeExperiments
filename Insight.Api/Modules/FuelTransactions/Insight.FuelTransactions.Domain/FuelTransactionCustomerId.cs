using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionCustomerId : ValueObject
    {
        public Guid Value { get; private set; }

        private FuelTransactionCustomerId()
        {
            Value = Guid.Empty;
        }

        private FuelTransactionCustomerId(Guid value)
        {
            Value = value;
        }

        public static FuelTransactionCustomerId Create(Guid value)
        {
            return new FuelTransactionCustomerId(value);
        }

        public static FuelTransactionCustomerId Empty()
        {
            return new FuelTransactionCustomerId();
        }
    }

}
