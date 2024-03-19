using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;

namespace Insight.OutgoingDeclarations.Domain.FuelConsumptionTransactionsExcelFile;

public sealed class FuelConsumptionTransactionsExcelFileFilteringParameters : ValueObject
{
    public FuelTransactionsBetweenDates DatePeriod { get; private set; } = FuelTransactionsBetweenDates.Empty();
    public IEnumerable<ProductName> ProductNames { get; private set; }
    public IEnumerable<FuelTransactionCustomerId> CustomerIds { get; private set; }

    private FuelConsumptionTransactionsExcelFileFilteringParameters()
    {
        ProductNames = new List<ProductName>();
        CustomerIds = new List<FuelTransactionCustomerId>();
    }

    private FuelConsumptionTransactionsExcelFileFilteringParameters(FuelTransactionsBetweenDates datePeriod, IEnumerable<ProductName> productNames, IEnumerable<FuelTransactionCustomerId> customerIds)
    {
        DatePeriod = datePeriod;
        ProductNames = productNames;
        CustomerIds = customerIds;
    }

    public static FuelConsumptionTransactionsExcelFileFilteringParameters Create(FuelTransactionsBetweenDates datePeriod, IEnumerable<ProductName> productNames, IEnumerable<FuelTransactionCustomerId> customerIds)
    {
        return new FuelConsumptionTransactionsExcelFileFilteringParameters(datePeriod, productNames, customerIds );
    }

    public static FuelConsumptionTransactionsExcelFileFilteringParameters Empty()
    {
        return new FuelConsumptionTransactionsExcelFileFilteringParameters();
    }
}