using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class FilterCompanyName : ValueObject
    {
        public string Value { get; private set; }

        private FilterCompanyName()
        {
            Value = string.Empty;
        }

        private FilterCompanyName(string value)
        {
            Value = value;
        }

        public static FilterCompanyName Create(string value)
        {
            return new FilterCompanyName(value);
        }

        public static FilterCompanyName Empty()
        {
            return new FilterCompanyName();
        }
    }
}
