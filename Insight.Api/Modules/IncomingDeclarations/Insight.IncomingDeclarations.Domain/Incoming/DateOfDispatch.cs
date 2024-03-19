using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class DateOfDispatch : ValueObject
    {
        public DateOnly Value { get; private set; }

        private DateOfDispatch()
        {
            Value = DateOnly.MinValue;
        }

        private DateOfDispatch(DateOnly value)
        {
            Value = value;
        }

        public static DateOfDispatch Create(DateOnly value)
        {
            return new DateOfDispatch(value);
        }

        public static DateOfDispatch Empty()
        {
            return new DateOfDispatch();
        }

        public static DateOfDispatch Today()
        {
            return new DateOfDispatch(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
