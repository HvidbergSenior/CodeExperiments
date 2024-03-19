using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactions
{
    public sealed class GetFuelConsumptionTransactionsResponse
    {
        [Required]
        public required IReadOnlyList<FuelConsumptionTransaction> Data { get; set; }
        [Required]
        public bool HasMoreTransactions { get; set; }
        [Required]
        public int TotalAmountOfTransactions { get; set; }
    }
    
    public sealed class FuelConsumptionTransaction
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string Date { get; set; }
        [Required]
        public required string Time { get; set; }
        [Required]
        public required string StationId { get; set; }
        [Required]
        public required string StationName { get; set; }
        [Required]
        public required string ProductNumber { get; set; }
        [Required]
        public required string ProductName { get; set; }
        [Required]
        public required string CustomerNumber { get; set; }
        [Required]
        public required string CustomerName { get; set; }
        [Required]
        public required string CardNumber { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required string Location { get; set; }
    }
}