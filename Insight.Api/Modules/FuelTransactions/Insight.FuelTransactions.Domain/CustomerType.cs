using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{   
    public sealed class CustomerType : ValueObject
    {
        public string Value { get; private set; }

        private CustomerType()
        {
            Value = string.Empty;
        }

        private CustomerType(string value)
        {
            Value = value;
        }

        public static CustomerType Create(string value)
        {
            return new CustomerType(value);
        }

        public static CustomerType Empty()
        {
            return new CustomerType();
        }
    }
}
