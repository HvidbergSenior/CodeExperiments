using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class FilterCustomerName : ValueObject
    {
        public string Value { get; private set; }

        private FilterCustomerName()
        {
            Value = string.Empty;
        }

        private FilterCustomerName(string value)
        {
            Value = value;
        }

        public static FilterCustomerName Create(string value)
        {
            return new FilterCustomerName(value);
        }

        public static FilterCustomerName Empty()
        {
            return new FilterCustomerName();
        }
    }
}
