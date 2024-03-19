using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class FilterProductName : ValueObject
    {
        public string Value { get; private set; }

        private FilterProductName()
        {
            Value = string.Empty;
        }

        private FilterProductName(string value)
        {
            Value = value;
        }

        public static FilterProductName Create(string value)
        {
            return new FilterProductName(value);
        }

        public static FilterProductName Empty()
        {
            return new FilterProductName();
        }
    }
}
