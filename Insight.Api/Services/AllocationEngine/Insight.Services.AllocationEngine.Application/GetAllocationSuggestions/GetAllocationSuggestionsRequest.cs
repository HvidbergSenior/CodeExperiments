using Insight.FuelTransactions.Domain;

namespace Insight.Services.AllocationEngine.Application.GetAllocationSuggestions
{
    public class GetAllocationSuggestionsRequest
    {
        public GetAllocationSuggestionsRequest(FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, string product, string country, string placeOfDispatch)
        {
            CustomerId = customerId.Value;
            StartDate = startDate;
            EndDate = endDate;
            Product = product;
            Country = country;
            PlaceOfDispatch = placeOfDispatch;
        }

        public Guid CustomerId { get; set; }
        public string Product { get; set; }
        public string Country { get; set; }
        public string PlaceOfDispatch { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }    
}
