using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using System.Globalization;

namespace Insight.FuelTransactions.Domain.Stock
{
    public sealed class StockTransaction : Entity
    {
        public StockTransactionId StockTransactionId { get; private set; } = StockTransactionId.Empty();
        public Location Location { get; private set; } = Location.Empty();
        public ProductNumber ProductNumber { get; private set; } = ProductNumber.Empty();
        public ProductName ProductName { get; private set; } = ProductName.Empty();
        public Quantity Quantity { get; private set; } = Quantity.Empty();
        public FuelTransactionCountry Country { get; private set; } = FuelTransactionCountry.Empty();
        public StockTransactionDate TransactionDate { get; private set; } = StockTransactionDate.Empty();
        public StockCompanyId CompanyId { get; private set; } = StockCompanyId.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();
        public StockAllocations Allocations { get; private set; } = StockAllocations.Empty();
        public decimal RemainingVolume => Quantity.Value - Allocations.TotalAllocatedVolume;
        public string LocationId => $"{Country.Value}:{Location.Value}";
        public string ItemHash => HashHelpers.GetHashCode(Location.Value, ProductNumber.Value, CompanyId.Value, TransactionDate.Value.ToString("yyyy-MM", CultureInfo.InvariantCulture));
        private StockTransaction()
        {
            Id = StockTransactionId.Value;
        }

        private StockTransaction(StockTransactionId stockTransactionId, Location location, ProductNumber productNumber, ProductName productName, Quantity quantity, StockTransactionDate stockTransactionDate, StockCompanyId stockCompanyId, CompanyName companyName)
        {
            StockTransactionId = stockTransactionId;
            Location = location;
            ProductNumber = productNumber;
            ProductName = productName;
            Quantity = quantity;
            TransactionDate = stockTransactionDate;
            CompanyId = stockCompanyId;
            CompanyName = companyName;
        }

        public static StockTransaction Create(StockTransactionId stockTransactionId, Location location, ProductNumber productNumber, ProductName productName, Quantity quantity, StockTransactionDate stockTransactionDate, StockCompanyId stockCompanyId, CompanyName companyName)
        {
            return new StockTransaction(stockTransactionId, location, productNumber, productName, quantity, stockTransactionDate, stockCompanyId, companyName);
        }
    }
}
