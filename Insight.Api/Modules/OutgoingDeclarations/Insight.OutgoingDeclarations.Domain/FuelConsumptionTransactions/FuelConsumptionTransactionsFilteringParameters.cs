using System.Collections.ObjectModel;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;

namespace Insight.OutgoingDeclarations.Domain.FuelConsumptionTransactions;

public sealed class FuelConsumptionTransactionsFilteringParameters : ValueObject
{
    public FuelTransactionsBetweenDates DatePeriod { get; private set; } = FuelTransactionsBetweenDates.Empty();
    public Insight.FuelTransactions.Domain.PaginationParameters PaginationParameters { get; private set; } = Insight.FuelTransactions.Domain.PaginationParameters.Empty();
    public IEnumerable<ProductName> ProductNames { get; private set; }
    public IEnumerable<FuelTransactionCustomerId> CustomerIds { get; private set; }
    public Insight.FuelTransactions.Domain.SortingParameters SortingParameters { get; private set; } = Insight.FuelTransactions.Domain.SortingParameters.Empty();
    
    private FuelConsumptionTransactionsFilteringParameters()
    {
        ProductNames = new List<ProductName>();
        CustomerIds = new List<FuelTransactionCustomerId>();
    }
    
    private FuelConsumptionTransactionsFilteringParameters(FuelTransactionsBetweenDates datePeriod, Insight.FuelTransactions.Domain.PaginationParameters paginationParameters, IEnumerable<ProductName> productNames, IEnumerable<FuelTransactionCustomerId> customerIds, Insight.FuelTransactions.Domain.SortingParameters sortingParameters)
    {
        DatePeriod = datePeriod;
        PaginationParameters = paginationParameters;
        ProductNames = productNames;
        CustomerIds = customerIds;
        SortingParameters = sortingParameters;
    }

    public static FuelConsumptionTransactionsFilteringParameters Create(FuelTransactionsBetweenDates datePeriod, Insight.FuelTransactions.Domain.PaginationParameters paginationParameters, IEnumerable<ProductName> productNames, IEnumerable<FuelTransactionCustomerId> customerIds, Insight.FuelTransactions.Domain.SortingParameters sortingParameters)
    {
        return new FuelConsumptionTransactionsFilteringParameters(datePeriod, paginationParameters, productNames, customerIds, sortingParameters);
    }

    public static FuelConsumptionTransactionsFilteringParameters Empty()
    {
        return new FuelConsumptionTransactionsFilteringParameters();
    }
}