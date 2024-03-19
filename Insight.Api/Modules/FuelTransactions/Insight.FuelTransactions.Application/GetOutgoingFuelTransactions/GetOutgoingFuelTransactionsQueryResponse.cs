using System.ComponentModel.DataAnnotations;

namespace Insight.FuelTransactions.Application.GetOutgoingFuelTransactions
{
    public sealed class GetOutgoingFuelTransactionsQueryResponse
    {
        [Required]
        public bool HasMoreOutgoingFuelTransactions { get; set; }
        [Required]
        public int TotalAmountOfOutgoingFuelTransactions { get; set; }
        [Required]
        public decimal TotalQuantity { get; set; }
        [Required]
        public IReadOnlyList<OutgoingFuelTransactionResponse> OutgoingFuelTransactions { get; private set; }

        public GetOutgoingFuelTransactionsQueryResponse(IReadOnlyList<OutgoingFuelTransactionResponse> outgoingFuelTransactions, bool hasMoreOutgoingFuelTransactions, int totalAmountOfOutgoingFuelTransactions, decimal totalQuantity)
        {
            OutgoingFuelTransactions = outgoingFuelTransactions;
            TotalAmountOfOutgoingFuelTransactions = totalAmountOfOutgoingFuelTransactions;
            HasMoreOutgoingFuelTransactions = hasMoreOutgoingFuelTransactions;
            TotalQuantity = totalQuantity;
        }
    }

    public sealed class OutgoingFuelTransactionResponse
    {
        [Required]
        public string Id { get; private set; } = string.Empty;

        [Required]
        public Guid CustomerId { get; private set; } = Guid.Empty;

        [Required]
        public string ProductNumber { get; private set; } = string.Empty;
        [Required]
        public string ProductName { get; private set; } = string.Empty;
        [Required]
        public string StationName { get; private set; } = string.Empty;
        [Required]
        public decimal Quantity { get; private set; }
        [Required]
        public string CustomerNumber { get; private set; } = string.Empty;
        [Required]
        public string CustomerName { get; private set; } = string.Empty;
        [Required]
        public string Country { get; private set; } = string.Empty;
        [Required]
        public string Location { get; private set; } = string.Empty;
        [Required]
        public string CustomerType { get; private set; } = string.Empty;
        [Required]
        public string CustomerSegment { get; private set; } = string.Empty;
        [Required]
        public decimal AllocatedQuantity { get; private set; }
        [Required]
        public decimal AlreadyAllocatedPercentage { get; private set; }
        [Required]
        public decimal MissingAllocationQuantity { get; private set; }

        [Required]
        public string LocationId { get; private set; } = string.Empty;

        public OutgoingFuelTransactionResponse(string id,
                                               Guid customerId,
                                               string productNumber,
                                               string stationName,
                                               decimal quantity,
                                               string customerNumber,
                                               string country,
                                               string customerName,
                                               string productName,
                                               string location,
                                               string locationId,
                                               string customerType,
                                               string customerSegment,
                                               decimal allocatedQuantity,
                                               decimal alreadyAllocatedPercentage,
                                               decimal missingAllocationQuantity)
        {
            Id = id;
            CustomerId = customerId;
            ProductNumber = productNumber;
            StationName = stationName;
            Quantity = quantity;
            CustomerNumber = customerNumber;
            Country = country;
            CustomerName = customerName;
            ProductName = productName;
            Location = location;
            LocationId = locationId;
            CustomerType = customerType;
            CustomerSegment = customerSegment;
            AllocatedQuantity = allocatedQuantity;
            AlreadyAllocatedPercentage = alreadyAllocatedPercentage;
            MissingAllocationQuantity = missingAllocationQuantity;
        }        
    }   
}
