using Insight.BuildingBlocks.Domain;

namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class AutoAllocationFilteringParameters : ValueObject
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; } 
        public FilterProductName FilterProductName { get; private set; } = FilterProductName.Empty();
        public FilterCustomerName FilterCustomerName { get; private set; } = FilterCustomerName.Empty();
        public FilterCompanyName FilterCompanyName { get; private set; } = FilterCompanyName.Empty();

        private AutoAllocationFilteringParameters()
        {
            //Left empty for serialization purposes
        }

        private AutoAllocationFilteringParameters(DateOnly startDate, DateOnly endDate, FilterProductName filterProductName, FilterCompanyName filterCompanyName, FilterCustomerName filterCustomerName)
        {
            StartDate = startDate;
            EndDate = endDate;
            FilterProductName = filterProductName;
            FilterCompanyName = filterCompanyName;
            FilterCustomerName = filterCustomerName;
        }

        public static AutoAllocationFilteringParameters Create(DateOnly startDate, DateOnly endDate, FilterProductName filterProductName, FilterCompanyName filterCompanyName, FilterCustomerName filterCustomerName)
        {
            return new AutoAllocationFilteringParameters(startDate, endDate, filterProductName, filterCompanyName, filterCustomerName);
        }

        public static AutoAllocationFilteringParameters Empty()
        {
            return new AutoAllocationFilteringParameters();
        }
    }
}