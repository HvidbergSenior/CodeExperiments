using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Test.Helpers.FuelConsumption;

public class GetConsumptionStatsTests
{
    [Fact]
    public void ConsumptionStatTestCalculationIsCorrect()
    {
        //Arrange
        var transactions = new List<GroupedFuelTransaction>()
        {
            GroupedFuelTransaction.Create(
                FuelTransactionDate.Create("2022-5-1"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.Diesel)),
                FuelTransactions.Domain.Quantity.Create(new decimal(100.0)), ProductDescription.Create(ProductNameEnumeration.Diesel + " Description")),
            GroupedFuelTransaction.Create(
                FuelTransactionDate.Create("2022-5-2"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.Hvo100)),
                FuelTransactions.Domain.Quantity.Create(new decimal(200.0)), ProductDescription.Create(ProductNameEnumeration.Hvo100 + " Description")),
            GroupedFuelTransaction.Create(
                FuelTransactionDate.Create("2022-5-3"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.Petrol)),
                FuelTransactions.Domain.Quantity.Create(new decimal(300.0)), ProductDescription.Create(ProductNameEnumeration.Petrol + " Description")),
            GroupedFuelTransaction.Create(
                FuelTransactionDate.Create("2022-5-4"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.B100)),
                FuelTransactions.Domain.Quantity.Create(new decimal(400.0)), ProductDescription.Create(ProductNameEnumeration.B100 + " Description"))
        };
        const double expectedRenewables = 200.0 + 400.0;
        const double expectedNonRenewables = 100.0 + 300.0;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionStats(getGroupedFuelTransactionsDtos);

        //Assert
        Assert.Equal(Convert.ToInt32(expectedNonRenewables), r.TotalConsumptionFossilFuels);
        Assert.Equal(Convert.ToInt32(expectedRenewables), r.TotalConsumptionRenewableFuels);
        Assert.Equal(Convert.ToInt32(expectedNonRenewables + expectedRenewables), r.TotalConsumptionAllFuels);
        Assert.Equal(Convert.ToInt32(expectedRenewables*100/(expectedRenewables+expectedNonRenewables)), r.ConsumptionTotalForCircle);
        Assert.Equal(Convert.ToInt32(expectedRenewables),
            r.Data[r.GeneralFuelTypes.ToList().IndexOf("Renewable fuel")]);
        Assert.Equal(Convert.ToInt32(expectedNonRenewables),
            r.Data[r.GeneralFuelTypes.ToList().IndexOf("Fossil fuel")]);
    }
}