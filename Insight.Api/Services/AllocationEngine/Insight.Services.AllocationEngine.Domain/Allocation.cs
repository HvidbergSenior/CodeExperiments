using Insight.BuildingBlocks.Domain;
using Insight.Customers.Domain;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain.Converters;
using Newtonsoft.Json;
namespace Insight.Services.AllocationEngine.Domain
{
    public sealed class Allocation : ValueObject
    {
        private Allocation()
        {
            // left empty
        }
        public CustomerDetails CustomerDetails { get; private set; } = CustomerDetails.Empty();
        public FuelTransactionCountry FuelTransactionCountry { get; private set; } = FuelTransactionCountry.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public AllocationId AllocationId { get; private set; } = AllocationId.Empty();
        [JsonConverter(typeof(IncomingDeclarationIdAndQuantityCollectionConverter))]
        public IncomingDeclarationIdAndQuantityCollection IncomingDeclarations { get; set; } = new IncomingDeclarationIdAndQuantityCollection();
        public decimal AllocatedSum => IncomingDeclarations.Sum(c => c.Value.Value); // TODO: Add customerid, location id etc. to this. to get allocation stats
        public FuelTransactionId[] FuelTransactionIds { get; private set; } = Array.Empty<FuelTransactionId>();
        public string[] Warnings { get; private set; } = Array.Empty<string>();
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        private Allocation(CustomerDetails customerDetails, FuelTransactionCountry fuelTransactionCountry, ProductName productName, DateOnly startDate, DateOnly endDate)
        {   
            AllocationId = AllocationId.Create(Guid.NewGuid());
            CustomerDetails = customerDetails;
            FuelTransactionCountry = fuelTransactionCountry;
            StartDate = startDate;
            EndDate = endDate;
            ProductName = productName;
        }
        public static Allocation Create(CustomerDetails customerDetails, FuelTransactionCountry fuelTransactionCountry, ProductName productName, DateOnly startDate, DateOnly endDate)
        {
            return new Allocation(customerDetails, fuelTransactionCountry, productName, startDate, endDate);
        }
        public void SetAllocations(IncomingDeclarationIdAndQuantityCollection incomingDeclarations)
        {
            IncomingDeclarations = incomingDeclarations;
        }
        public void SetTransactionIds(FuelTransactionId[] fuelTransactionIds)
        {
            FuelTransactionIds = fuelTransactionIds;
        }
        public void SetWarnings(string[] warnings)
        {
            Warnings = warnings;
        }
        public static Allocation Empty()
        {
            return new Allocation(CustomerDetails.Empty(), FuelTransactionCountry.Empty(), ProductName.Empty(), DateOnly.MinValue, DateOnly.MaxValue);
        }
    }
}
