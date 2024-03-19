using System.ComponentModel.DataAnnotations;

namespace Insight.FuelTransactions.Application.CreateStockTransaction
{
    public class CreateStockTransactionRequest
    {
        [Required]
        public required string ProductNumber { get; set; }
        [Required]
        public required Guid CompanyId { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required DateOnly TransactionDate { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string Location { get; set; }
    }
}
