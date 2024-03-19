using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class DateOfCreation : ValueObject
    {
        public DateOnly Value { get; private set; }

        private DateOfCreation()
        {
            Value = DateOnly.MinValue;
        }

        private DateOfCreation(DateOnly value)
        {
            Value = value;
        }

        public static DateOfCreation Create(DateOnly value)
        {
            return new DateOfCreation(value);
        }

        public static DateOfCreation Empty()
        {
            return new DateOfCreation();
        }

        public static DateOfCreation Today()
        {
            return new DateOfCreation(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}