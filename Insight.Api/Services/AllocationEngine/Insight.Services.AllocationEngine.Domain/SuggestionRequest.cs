using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.Services.AllocationEngine.Domain
{
    public class SuggestionRequest : ValueObject
    {
        public Product Product { get; private set; } = Product.Empty();
        public Country Country { get; private set; } = Country.Empty();
        public PlaceOfDispatch PlaceOfDispatch { get; private set; } = PlaceOfDispatch.Empty();
        public bool IsOrderDescending { get; private set; }
        public OrderByProperty OrderByProperty { get; private set; } = OrderByProperty.Empty();
        public FuelTransactionCustomerId CustomerId { get; private set; } = FuelTransactionCustomerId.Empty();
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        public SuggestionRequest(FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, Product product, Country country, PlaceOfDispatch placeOfDispatch, bool isOrderDescending, OrderByProperty orderByProperty)
        {
            CustomerId = fuelTransactionCustomerId;
            StartDate = startDate;
            EndDate = endDate;
            Product = product;
            Country = country;
            PlaceOfDispatch = placeOfDispatch;
            IsOrderDescending = isOrderDescending;
            OrderByProperty = orderByProperty;
        }

        public static SuggestionRequest Create(FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, Product product, Country country, PlaceOfDispatch placeOfDispatch, bool isOrderDescending, OrderByProperty orderByProperty)
        {
            return new SuggestionRequest(fuelTransactionCustomerId, startDate, endDate, product, country, placeOfDispatch, isOrderDescending, orderByProperty);
        }
    }
}
