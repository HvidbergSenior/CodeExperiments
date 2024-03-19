namespace Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery
{
    public class GetGroupedFuelTransactionsDto
    {
        public List<GetGroupedFuelTransactionDto> GroupedFuelTransactionDto { get; private set; }

        public GetGroupedFuelTransactionsDto(List<GetGroupedFuelTransactionDto> groupedFuelTransactionDto)
        {
            GroupedFuelTransactionDto = groupedFuelTransactionDto;
        }
    }

    public sealed class GetGroupedFuelTransactionDto
    {
        public string FuelTransactionDate { get; private set; }
        public string ProductName { get; private set; }
        public decimal Quantity { get; private set; }
        public string ProductDescription { get; private set; }


        public GetGroupedFuelTransactionDto(
            string fuelTransactionDate,
            string productName,
            decimal quantity,
            string productDescription
        )
        {
            FuelTransactionDate = fuelTransactionDate;
            ProductName = productName;
            Quantity = quantity;
            ProductDescription = productDescription;
        }
    }
}