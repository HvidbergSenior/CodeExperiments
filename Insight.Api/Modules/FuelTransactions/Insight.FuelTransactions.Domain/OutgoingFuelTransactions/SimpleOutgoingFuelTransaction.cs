namespace Insight.FuelTransactions.Domain.OutgoingFuelTransactions
{
    public sealed class SimpleOutgoingFuelTransaction
    {
        public string StationNumber { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public int ItemCount { get; set; }
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Guid CustomerId { get; set; } = Guid.Empty;
        public string Country { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ShipToLocation { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerSegment { get; set; } = string.Empty;
        public decimal AllocatedQuantity { get; set; }
        public decimal MissingAllocationQuantity { get; set; }
        public decimal AlreadyAllocatedPercentage { get; set; }        
        public OutgoingFuelTransaction AsOutgoingFuelTransaction()
        {
            return OutgoingFuelTransaction.Create(FuelTransactionCustomerId.Create(CustomerId), Domain.CompanyName.Create(CompanyName), Domain.StationNumber.Create(StationNumber), Domain.StationName.Create(StationName), Domain.ProductNumber.Create(ProductNumber),
                BuildingBlocks.Domain.ProductName.Create(ProductName), Domain.Quantity.Create(Quantity), Insight.BuildingBlocks.Domain.CustomerNumber.Create(CustomerNumber), Insight.BuildingBlocks.Domain.CustomerName.Create(CustomerName),Domain.FuelTransactionCountry.Create(Country), Domain.ItemCount.Create(ItemCount),
                Domain.Location.Create(Location),Domain.CustomerType.Create(CustomerType), Domain.CustomerSegment.Create(CustomerSegment),  Domain.LocationId.Create(LocationId), Domain.Quantity.Create(AllocatedQuantity), Domain.Percentage.Create(AlreadyAllocatedPercentage), Domain.Quantity.Create(MissingAllocationQuantity), Domain.Quantity.Create(TotalQuantity), Domain.ShipToLocation.Create(ShipToLocation));
        }
    }
}
