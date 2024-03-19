using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Test.Helpers.FuelConsumption;

public class GetConsumptionPerProductTests
{
    [Fact]
    public void ConsumptionPerProductIsCorrect()
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
                FuelTransactionDate.Create("2022-5-3"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.Diesel)),
                FuelTransactions.Domain.Quantity.Create(new decimal(100.0)), ProductDescription.Create(ProductNameEnumeration.Diesel + " Description")),
            GroupedFuelTransaction.Create(
                FuelTransactionDate.Create("2022-5-4"),
                ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(ProductNameEnumeration.Petrol)),
                FuelTransactions.Domain.Quantity.Create(new decimal(400.0)), ProductDescription.Create(ProductNameEnumeration.Petrol + " Description"))
        };
        const decimal expectedDiesel = 100.0m + 100.0m;
        const decimal expectedPetrol = 300.0m + 400.0m;
        const decimal expectedHvo100 = 200.0m;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionPerProduct(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto);

        //Assert
        Assert.Equal(3, r.Data.Count());
        Assert.Single(r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.Equal(expectedDiesel, r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Diesel).Select(t => t.Value).FirstOrDefault());
        Assert.Single(r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.Equal(expectedPetrol, r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Petrol).Select(t => t.Value).FirstOrDefault());
        Assert.Single(r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Hvo100).ToList());
        Assert.Equal(expectedHvo100, r.Data.ToList().Where(t => t.ProductNameEnumeration == ProductNameEnumeration.Hvo100).Select(t => t.Value).FirstOrDefault());
    }
}