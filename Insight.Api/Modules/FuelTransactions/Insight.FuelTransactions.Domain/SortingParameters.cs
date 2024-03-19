using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class SortingParameters : ValueObject
    {
        public bool IsOrderDescending { get; private set; }
        public string OrderByProperty { get; private set; }


        private SortingParameters()
        {
            IsOrderDescending = default;
            OrderByProperty = string.Empty;
        }

        private SortingParameters(bool isOrderDescending, string orderByProperty)
        {
            IsOrderDescending = isOrderDescending;
            OrderByProperty = orderByProperty;
        }

        public static SortingParameters Create(bool isOrderDescending, string orderByProperty)
        {
            return new SortingParameters(isOrderDescending, orderByProperty);
        }

        public static SortingParameters Empty()
        {
            return new SortingParameters();
        }
    }
}
