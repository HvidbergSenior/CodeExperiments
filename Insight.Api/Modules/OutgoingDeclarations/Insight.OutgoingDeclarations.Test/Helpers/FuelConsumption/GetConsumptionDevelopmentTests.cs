using System.Globalization;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.OutgoingDeclarations.Domain;
using JasperFx.Core;

namespace Insight.OutgoingDeclarations.Test.Helpers.FuelConsumption;

public sealed class TransactionGeneratorConfig
{
    public ProductNameEnumeration ProductNameEnumeration { get; set; } = ProductNameEnumeration.Unknown;
    public DateOnly DateStart { get; set; } = DateOnly.MinValue;
    public int DaysGenerate { get; set; }
    public decimal QuantityStart { get; set; }
    public decimal QuantityStep { get; set; }
    public string ProductDescription { get; set; } = string.Empty;
}

public class GetConsumptionDevelopmentTests
{
    [Fact]
    public void ConsumptionDevelopmentDailyLessThanMaxColumnsIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 100,
                QuantityStep = 0,
                ProductDescription = ProductNameEnumeration.Diesel + " Description"
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 200,
                QuantityStep = 5,
                ProductDescription = ProductNameEnumeration.Petrol + " Description"
                }
        });
        int maxColumns = 12;

        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();
        
        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(10, r.Categories.Count);
        Assert.Equal(2, r.Series.Count);
        Assert.Equal(10, r.Series[0].Data.Count);
        Assert.Equal(10, r.Series[1].Data.Count);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList(), 100, 0, 10));
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList(), 200, 5, 10));
    }

    [Fact]
    public void ConsumptionDevelopmentDailyWithHolesIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 100,
                QuantityStep = 0
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 5,
                QuantityStart = 200,
                QuantityStep = 5
            }
        });
        int maxColumns = 12;
        
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(10, r.Categories.Count);
        Assert.Equal(2, r.Series.Count);
        Assert.Equal(10, r.Series[0].Data.Count);
        Assert.Equal(10, r.Series[1].Data.Count);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList(), 100, 0, 10));
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList(), 200, 5, 5));
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList().Skip(5).Take(5).ToList(), 0, 0, 5));
    }

    [Fact]
    public void ConsumptionDevelopmentDailyDatesAreCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 100,
                QuantityStep = 0
            }
        });
        int maxColumns = 12;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(10, r.Categories.Count);
        Assert.Equal("1 JAN", r.Categories[0]);
        Assert.Equal("2 JAN", r.Categories[1]);
        Assert.Equal("3 JAN", r.Categories[2]);
        Assert.Equal("4 JAN", r.Categories[3]);
        Assert.Equal("5 JAN", r.Categories[4]);
        Assert.Equal("6 JAN", r.Categories[5]);
        Assert.Equal("7 JAN", r.Categories[6]);
        Assert.Equal("8 JAN", r.Categories[7]);
        Assert.Equal("9 JAN", r.Categories[8]);
        Assert.Equal("10 JAN", r.Categories[9]);
    }

    [Fact]
    public void ConsumptionDevelopmentMonthlyWhenMoreThanMaxColumnsIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 100,
                QuantityStep = 0
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 10,
                QuantityStart = 200,
                QuantityStep = 5
            }
        });
        int maxColumns = 9;
        int expectedSumDiesel = 100 * 10;
        int expectedSumPetrol = 200 * 10 + 5 * GetGaussSum(10 - 1);

        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Single(r.Categories);
        Assert.Equal(2, r.Series.Count);
        Assert.Single(r.Series[0].Data);
        Assert.Single(r.Series[1].Data);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList(), expectedSumDiesel, 0, 1));
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.True(ValidateValuesOnData(
            r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList(), expectedSumPetrol, 0, 1));
    }

    [Fact]
    public void ConsumptionDevelopmentMonthlySeveralMonthsIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 75,
                QuantityStart = 100,
                QuantityStep = 0
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 75,
                QuantityStart = 200,
                QuantityStep = 5
            }
        });
        int maxColumns = 30;
        var expectedSumFirstMonthDiesel = 100 * 31;
        var expectedSumSecondMonthDiesel = 100 * 29;
        var expectedSumThirdMonthDiesel = 100 * (75-31-29);
        var expectedSumFirstMonthPetrol = 200*31+5*GetGaussSum(31-1);
        var expectedSumSecondMonthPetrol = (200+5*31)*29+5*GetGaussSum(29-1);
        var expectedSumThirdMonthPetrol = (200+5*(31+29))*(75-31-29)+5*GetGaussSum(75-31-29-1);

        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(3, r.Categories.Count());
        Assert.Equal(2, r.Series.Count);
        Assert.Equal(3, r.Series[0].Data.Count);
        Assert.Equal(3, r.Series[1].Data.Count);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.Equal(expectedSumFirstMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[1]);
        Assert.Equal(expectedSumThirdMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[2]);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.Equal(expectedSumFirstMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[1]);
        Assert.Equal(expectedSumThirdMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[2]);
    }

    [Fact]
    public void ConsumptionDevelopmentMonthlySeveralMonthsWithHolesIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 75,
                QuantityStart = 100,
                QuantityStep = 0
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 20,
                QuantityStart = 200,
                QuantityStep = 5
            }
        });
        int maxColumns = 30;
        var expectedSumFirstMonthDiesel = 100 * 31;
        var expectedSumSecondMonthDiesel = 100 * 29;
        var expectedSumThirdMonthDiesel = 100 * (75-31-29);
        var expectedSumFirstMonthPetrol = 200*20+5*GetGaussSum(20-1);
        var expectedSumSecondMonthPetrol = 0; //Note that there are holes in the data, this is missing so it should be zero
        var expectedSumThirdMonthPetrol = 0;  //Note that there are holes in the data, this is missing so it should be zero

        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(3, r.Categories.Count());
        Assert.Equal(2, r.Series.Count);
        Assert.Equal(3, r.Series[0].Data.Count);
        Assert.Equal(3, r.Series[1].Data.Count);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.Equal(expectedSumFirstMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[1]);
        Assert.Equal(expectedSumThirdMonthDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[2]);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.Equal(expectedSumFirstMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[1]);
        Assert.Equal(expectedSumThirdMonthPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[2]);
    }

    [Fact]
    public void ConsumptionDevelopmentMonthlyDatesAreCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 360,
                QuantityStart = 100,
                QuantityStep = 0
            }
        });
        int maxColumns = 30;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(12, r.Categories.Count);
        Assert.Equal("JAN '20", r.Categories[0]);
        Assert.Equal("FEB '20", r.Categories[1]);
        Assert.Equal("MAR '20", r.Categories[2]);
        Assert.Equal("APR '20", r.Categories[3]);
        Assert.Equal("MAY '20", r.Categories[4]);
        Assert.Equal("JUN '20", r.Categories[5]);
        Assert.Equal("JUL '20", r.Categories[6]);
        Assert.Equal("AUG '20", r.Categories[7]);
        Assert.Equal("SEP '20", r.Categories[8]);
        Assert.Equal("OCT '20", r.Categories[9]);
        Assert.Equal("NOV '20", r.Categories[10]);
        Assert.Equal("DEC '20", r.Categories[11]);
    }

    [Fact]
    public void ConsumptionDevelopmentIStillUseMonthsWith364Days()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2020,1,1),
                DaysGenerate = 364,
                QuantityStart = 100,
                QuantityStep = 0
            },
        });
        int maxColumns = 30;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(12, r.Categories.Count);
        Assert.Single(r.Series);
        Assert.Equal(12, r.Series[0].Data.Count);
    }

    [Fact]
    public void ConsumptionDevelopmentIStartWithYearsAfter366Days()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2022,1,1),
                DaysGenerate = 366,
                QuantityStart = 100,
                QuantityStep = 0
            },
        });
        int maxColumns = 30;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(13, r.Categories.Count());
        Assert.Single(r.Series);
        Assert.Equal(13, r.Series[0].Data.Count);
    }

    [Fact]
    public void ConsumptionDevelopmentYearlySeveralYearsIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2022,1,1),
                DaysGenerate = 400,
                QuantityStart = 100,
                QuantityStep = 0
            },
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Petrol,
                DateStart = new DateOnly(2022,1,1),
                DaysGenerate = 400,
                QuantityStart = 200,
                QuantityStep = 5
            }
        });
        int maxColumns = 30;
        var expectedSumFirstYearDiesel = 100*365;
        var expectedSumSecondYearDiesel = 100*(400-365);
        var expectedSumFirstYearPetrol = 200*365+5*GetGaussSum(365-1);
        var expectedSumSecondYearPetrol = (200+5*365)*(400-365)+5*GetGaussSum(400-365-1);
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(2, r.Categories.Count());
        Assert.Equal(2, r.Series.Count);
        Assert.Equal(2, r.Series[0].Data.Count);
        Assert.Equal(2, r.Series[1].Data.Count);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel).ToList());
        Assert.Equal(expectedSumFirstYearDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondYearDiesel, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Diesel)!.Data.ToList()[1]);
        Assert.Single(r.Series.ToList().Where(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol).ToList());
        Assert.Equal(expectedSumFirstYearPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[0]);
        Assert.Equal(expectedSumSecondYearPetrol, r.Series.ToList().FirstOrDefault(x => x.ProductNameEnumeration == ProductNameEnumeration.Petrol)!.Data.ToList()[1]);
    }

        [Fact]
    public void ConsumptionDevelopmentYearlySeveralYearsDateIsCorrect()
    {
        //Arrange
        var transactions = GenerateTransactions(new List<TransactionGeneratorConfig>()
        {
            new TransactionGeneratorConfig()
            {
                ProductNameEnumeration = ProductNameEnumeration.Diesel,
                DateStart = new DateOnly(2022,1,1),
                DaysGenerate = 365*3 + 100,
                QuantityStart = 100,
                QuantityStep = 0
            }
        });
        int maxColumns = 30;
        var getGroupedFuelTransactionDtos = new List<GetGroupedFuelTransactionDto>();

        foreach (var transaction in transactions)
        {
            getGroupedFuelTransactionDtos.Add(new GetGroupedFuelTransactionDto(transaction.FuelTransactionDate.Value, transaction.ProductName.Value, transaction.Quantity.Value, transaction.ProductDescription.Value));
        }

        var getGroupedFuelTransactionsDtos = new GetGroupedFuelTransactionsDto(getGroupedFuelTransactionDtos);
        //Act
        var r = Insight.OutgoingDeclarations.Application.Helpers.FuelConsumptionHelper
            .GetConsumptionDevelopment(getGroupedFuelTransactionsDtos.GroupedFuelTransactionDto, MaxColumns.Create(maxColumns));

        //Assert
        Assert.Equal(4, r.Categories.Count());
        Assert.Equal("2022", r.Categories[0]);
        Assert.Equal("2023", r.Categories[1]);
        Assert.Equal("2024", r.Categories[2]);
        Assert.Equal("2025", r.Categories[3]);
    }

    private IEnumerable<GroupedFuelTransaction> GenerateTransactions(IEnumerable<TransactionGeneratorConfig> generators)
    {
        var transactions = new List<GroupedFuelTransaction>();
        foreach (var generator in generators)
        {
            for (var dayOffset = 0; dayOffset < generator.DaysGenerate; dayOffset++)
            {
                var newDate = generator.DateStart.AddDays(dayOffset);
                var dateStr = newDate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                decimal quantity = generator.QuantityStart + generator.QuantityStep * dayOffset;
                var productDescription = generator.ProductDescription;
                transactions.Add(GroupedFuelTransaction.Create(FuelTransactionDate.Create(dateStr), ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(generator.ProductNameEnumeration)), FuelTransactions.Domain.Quantity.Create(quantity), ProductDescription.Create(productDescription)));
            }
        }

        return transactions;
    }

    private bool ValidateValuesOnData(IReadOnlyList<int> data, int valueStart, int stepSize, int maxDays)
    {
        var ok = true;
        for(var daysOffset = 0; daysOffset < maxDays; daysOffset++)
        {
            if ( (valueStart + daysOffset*stepSize) != data[daysOffset])
            {
                ok = false;
                break;
            }
        }

        return ok;
    }

    private int GetGaussSum(int n) => n*(n+1)/2;
}