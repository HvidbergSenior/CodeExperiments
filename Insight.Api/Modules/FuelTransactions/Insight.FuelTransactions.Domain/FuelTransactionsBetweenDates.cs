using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain;

public sealed class FuelTransactionsBetweenDates : ValueObject
{
    public DateOnly FromDate { get; private set; }
    public DateOnly ToDate { get; private set; }

    private FuelTransactionsBetweenDates()
    {
        FromDate = default;
        ToDate = default;
    }

    private FuelTransactionsBetweenDates(DateOnly fromDate, DateOnly toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
    }

    public static FuelTransactionsBetweenDates Create(DateOnly fromDate, DateOnly toDate)
    {
        if (fromDate > toDate)
        {
            throw new ArgumentException("FromDate cannot be after ToDate", nameof(fromDate));
        }

        return new FuelTransactionsBetweenDates(fromDate, toDate);
    }

    public static FuelTransactionsBetweenDates Empty()
    {
        return new FuelTransactionsBetweenDates();
    }
}