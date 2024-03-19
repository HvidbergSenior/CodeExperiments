using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class DateOfIssuance : ValueObject
    {
        public DateOnly Value { get; private set; }

        private DateOfIssuance()
        {
            Value = DateOnly.MinValue;
        }

        private DateOfIssuance(DateOnly value)
        {
            Value = value;
        }

        public static DateOfIssuance Create(DateOnly value)
        {
            return new DateOfIssuance(value);
        }

        public static DateOfIssuance Empty()
        {
            return new DateOfIssuance();
        }

        public static DateOfIssuance Today()
        {
            return new DateOfIssuance(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
