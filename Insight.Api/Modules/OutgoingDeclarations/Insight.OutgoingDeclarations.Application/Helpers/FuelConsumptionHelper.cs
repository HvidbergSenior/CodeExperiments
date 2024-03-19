using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.Helpers;

public static class FuelConsumptionHelper
{
    private static string PrettifyDate(string date)
    {
        /*
             * Examples:
             * 2022-05-20 -> 20 APR
             * 2022-06-20 -> 20 JUN
             * 2022-06    -> JUN '22
             * 2022       -> 2022
             */

        string[] months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        var dateTrimmed = date.Trim();
        var dateOut = date;
        if (dateTrimmed.Length == 4)
        {
            //Yearly, nothing to do
            dateOut = dateTrimmed;
        }
        else if (dateTrimmed.Length == 7)
        {
            //Year-month
            var splitDate = dateTrimmed.Split("-").ToList();
            if (splitDate.Count == 2)
            {
                var yearOk = int.TryParse(splitDate[0], out var year);
                var monthOk = int.TryParse(splitDate[1], out var month);
                if (yearOk && year is >= 1900 and <= 2100
                           && monthOk && month is >= 1 and <= 12)
                {
                    dateOut = months[month - 1] + " '" + (year % 100);
                }
            }
        }
        else if (dateTrimmed.Length == 10)
        {
            //Year-month-day
            var splitDate = dateTrimmed.Split("-").ToList();
            if (splitDate.Count == 3)
            {
                var yearOk = int.TryParse(splitDate[0], out var year);
                var monthOk = int.TryParse(splitDate[1], out var month);
                var dayOk = int.TryParse(splitDate[2], out var day);
                if (yearOk && year is >= 1900 and <= 2100
                           && monthOk && month is >= 1 and <= 12
                           && dayOk && day is >= 1 and <= 31)
                {
                    dateOut = day + " " + months[month - 1];
                }
            }
        }

        return dateOut;
    }

    public static ConsumptionPerProduct GetConsumptionPerProduct(IEnumerable<GetGroupedFuelTransactionDto> transactions)
    {
        var data = transactions
            .GroupBy(g => g.ProductName)
            .Select(s =>
                new FuelConsumptionNameValuePair(ProductNameEnumerationExtensions.TranslatedStringToProductNameEnumeration(s.Key),
                    s.Sum(s => s.Quantity))).ToList();

        var consumptionPerProduct = new ConsumptionPerProduct()
        {
            Data = data
        };

        return consumptionPerProduct;
    }

    public static ConsumptionDevelopment GetConsumptionDevelopment(IEnumerable<GetGroupedFuelTransactionDto> transactions,
        MaxColumns maxColumns)
    {
        ConsumptionDevelopment c;
        var distinctDatesMonths = transactions.Select(t => t.FuelTransactionDate.Substring(0, 7)).Distinct().Count();

        if (distinctDatesMonths > 13)
        {
            //Yearly
            c = GetConsumptionDevelopmentTruncated(transactions, 4);
        }
        else
        {
            var distinctDatesDays = transactions.Select(t => t.FuelTransactionDate).Distinct().Count();
            if (distinctDatesDays > maxColumns.Value)
            {
                //Monthly
                c = GetConsumptionDevelopmentTruncated(transactions, 7);
            }
            else
            {
                //Daily
                c = GetConsumptionDevelopmentDaily(transactions);
            }
        }

        return c;
    }

    class FuelConsumptionTransactionPair
    {
        public string Date { get; init; } = string.Empty;
        public int Value { get; init; }
    }

    private static IEnumerable<int> GetTransactionSeriesExtended(IEnumerable<FuelConsumptionTransactionPair> points, IEnumerable<string> expectedDates)
    {
        var listWithHolesFilled = new List<int>();

        foreach (var expectedDate in expectedDates)
        {
            var point = points.FirstOrDefault(p => p.Date == expectedDate);
            if (point != null)
            {
                listWithHolesFilled.Add(point.Value);
            }
            else
            {
                listWithHolesFilled.Add(0);
            }
        }

        return listWithHolesFilled;
    }

    private static ConsumptionDevelopment GetConsumptionDevelopmentDaily(IEnumerable<GetGroupedFuelTransactionDto> transactions)
    {
        var categories = transactions.Select(t => t.FuelTransactionDate).Distinct().OrderBy(t => t).ToList();
        var series = new List<FuelConsumptionSeries>();
        var fuelTypes = transactions.Select(t => t.ProductName).Distinct().ToList();
        foreach (var fuelType in fuelTypes)
        {
            var dataForFuelTransactions = transactions.OrderBy(o => o.FuelTransactionDate)
                .Where(t => t.ProductName == fuelType);

            var dataForFuelPairs = dataForFuelTransactions.Select(t =>
               {
                   return new FuelConsumptionTransactionPair()
                   {
                       Date = t.FuelTransactionDate,
                       Value = Convert.ToInt32(t.Quantity)
                   };
               }).ToList();

            var dataForFuel = GetTransactionSeriesExtended(dataForFuelPairs, categories);

            series.Add(new FuelConsumptionSeries()
            {
                ProductNameEnumeration = ProductNameEnumerationExtensions.TranslatedStringToProductNameEnumeration(fuelType),
                Data = dataForFuel.ToList()
            });
        }

        var categoriesPretty = categories.Select(PrettifyDate).ToList();
        var c = new ConsumptionDevelopment()
        {
            Categories = categoriesPretty,
            Series = series
        };

        return c;
    }

    private static ConsumptionDevelopment GetConsumptionDevelopmentTruncated(
        IEnumerable<GetGroupedFuelTransactionDto> transactions, int charactersTruncate)
    {
        var categories = transactions.Select(t => t.FuelTransactionDate.Substring(0, charactersTruncate)).Distinct().OrderBy(t => t).ToList();
        var series = new List<FuelConsumptionSeries>();
        var fuelTypes = transactions.Select(t => t.ProductName).Distinct().ToList();
        foreach (var fuelType in fuelTypes)
        {
            var dataForFuelTransactions = transactions
                .Where(w => w.ProductName == fuelType)
                .OrderBy(o => o.FuelTransactionDate);

            var dataForFuelPairs = dataForFuelTransactions.GroupBy(g => g.FuelTransactionDate.Substring(0, charactersTruncate))
                .Select(s =>
                {
                    return new FuelConsumptionTransactionPair()
                    {
                        Date = s.First().FuelTransactionDate.Substring(0, charactersTruncate),
                        Value = s.Sum(s => Convert.ToInt32(s.Quantity))
                    };
                }).ToList();

            var dataForFuel = GetTransactionSeriesExtended(dataForFuelPairs, categories);

            series.Add(new FuelConsumptionSeries()
            {
                ProductNameEnumeration = ProductNameEnumerationExtensions.TranslatedStringToProductNameEnumeration(fuelType),
                Data = dataForFuel.ToList()
            });
        }

        var categoriesPretty = categories.Select(PrettifyDate).ToList();
        var c = new ConsumptionDevelopment()
        {
            Categories = categoriesPretty,
            Series = series
        };

        return c;
    }

    public static ConsumptionStats GetConsumptionStats(GetGroupedFuelTransactionsDto transactions)
    {
        var renewableSum = transactions.GroupedFuelTransactionDto
            .Where(w => ProductNameEnumerationExtensions.IsRenewable(ProductNameEnumerationExtensions.TranslatedStringToProductNameEnumeration(w.ProductName)))
            .Sum(s => s.Quantity);

        var nonRenewableSum = transactions.GroupedFuelTransactionDto
            .Where(w => !ProductNameEnumerationExtensions.IsRenewable(ProductNameEnumerationExtensions.TranslatedStringToProductNameEnumeration(w.ProductName)))
            .Sum(s => s.Quantity);

        var consumptionTotalForCircle = 0;
        if ((renewableSum != 0) && (renewableSum + nonRenewableSum) != 0)
        {
            consumptionTotalForCircle =
                Convert.ToInt32(new decimal(100.0) * renewableSum / (renewableSum + nonRenewableSum));
        }

        var c = new ConsumptionStats()
        {
            Data = new List<int>()
            {
                Convert.ToInt32(renewableSum), Convert.ToInt32(nonRenewableSum)
            },
            GeneralFuelTypes = new List<string>()
            {
                "Renewable fuel", "Fossil fuel"
            },
            TotalConsumptionFossilFuels = Convert.ToInt32(nonRenewableSum),
            TotalConsumptionRenewableFuels = Convert.ToInt32(renewableSum),
            TotalConsumptionAllFuels = Convert.ToInt32(renewableSum + nonRenewableSum),
            ConsumptionTotalForCircle = consumptionTotalForCircle
        };

        return c;
    }
}