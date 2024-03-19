using Insight.BuildingBlocks.Domain;
namespace Insight.FuelTransactions.Domain
{
    public sealed class CustomerSegment : ValueObject
    {
        public string Value { get; private set; }

        private CustomerSegment()
        {
            Value = string.Empty;
        }

        private CustomerSegment(string value)
        {
            Value = value;
        }

        public static CustomerSegment Create(string value)
        {
            return new CustomerSegment(value);
        }

        public static CustomerSegment Empty()
        {
            return new CustomerSegment();
        }
    }
}
