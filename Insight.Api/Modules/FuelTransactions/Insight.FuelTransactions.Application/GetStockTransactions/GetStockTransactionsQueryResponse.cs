using System.ComponentModel.DataAnnotations;

namespace Insight.FuelTransactions.Application.GetStockTransactions
{
    public sealed class GetStockTransactionsQueryResponse
    {
        [Required]
        public bool HasMoreStockTransactions { get; set; }
        [Required]
        public int TotalAmountOfStockTransactions { get; set; }
        [Required]
        public IReadOnlyList<StockTransactionResponse> StockTransactions { get; private set; }

        public GetStockTransactionsQueryResponse(IReadOnlyList<StockTransactionResponse> stockTransactions, bool hasMoreStockTransactions, int totalAmountOfStockTransactions)
        {
            StockTransactions = stockTransactions;
            TotalAmountOfStockTransactions = totalAmountOfStockTransactions;
            HasMoreStockTransactions = hasMoreStockTransactions;
        }
    }
    public sealed class StockTransactionResponse
    {
        [Required]
        public Guid Id { get; private set; } = Guid.Empty;
        [Required]
        public Guid CompanyId { get; private set; } = Guid.Empty;
        [Required]
        public string CompanyName { get; private set; } = string.Empty;
        [Required]
        public string ProductNumber { get; private set; } = string.Empty;
        [Required]
        public string ProductName { get; private set; } = string.Empty;
        [Required]
        public decimal Quantity { get; private set; }
        [Required]
        public string Country { get; private set; } = string.Empty;
        [Required]
        public string Location { get; private set; } = string.Empty;
        [Required]
        public decimal AllocatedQuantity { get; private set; }
        [Required]
        public decimal AlreadyAllocatedPercentage { get; private set; }
        [Required]
        public decimal MissingAllocationQuantity { get; private set; }
        [Required]
        public string LocationId { get; private set; } = string.Empty;

        public StockTransactionResponse(Guid id,
                                               Guid companyId,
                                               string companyName,
                                               string productNumber,
                                               string productName,
                                               decimal quantity,                                               
                                               string country,                                               
                                               string location,
                                               string locationId,
                                               decimal allocatedQuantity,
                                               decimal alreadyAllocatedPercentage,
                                               decimal missingAllocationQuantity)
        {
            Id = id;
            CompanyId = companyId;
            CompanyName = companyName;
            ProductNumber = productNumber;
            Quantity = quantity;
            Country = country;
            ProductName = productName;
            Location = location;
            LocationId = locationId;
            AllocatedQuantity = allocatedQuantity;
            AlreadyAllocatedPercentage = alreadyAllocatedPercentage;
            MissingAllocationQuantity = missingAllocationQuantity;
        }
    }
}
