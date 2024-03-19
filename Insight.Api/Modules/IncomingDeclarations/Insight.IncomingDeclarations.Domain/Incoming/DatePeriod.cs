using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class DatePeriod : ValueObject
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }


        private DatePeriod()
        {
            StartDate = DateOnly.MinValue;
            EndDate = DateOnly.MaxValue;
        }

        private DatePeriod(DateOnly startDate, DateOnly endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public static DatePeriod Create(DateOnly startDate, DateOnly endDate)
        {
            return new DatePeriod(startDate, endDate);
        }

        public static DatePeriod Empty()
        {
            return new DatePeriod(DateOnly.MinValue, DateOnly.MinValue);
        }

        public static DatePeriod Always()
        {
            return new DatePeriod(DateOnly.MinValue, DateOnly.MaxValue);
        }

        public bool IsDateIncluded(DateOnly date)
        {
            return date >= StartDate && date <= EndDate;
        }
    }
}
