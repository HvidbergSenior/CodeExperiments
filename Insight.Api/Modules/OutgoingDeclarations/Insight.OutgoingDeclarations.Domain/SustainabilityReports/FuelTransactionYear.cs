using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain.SustainabilityReports
{
    public sealed class FuelTransactionYear : ValueObject
    {
        public int Value { get; private set; }

        private FuelTransactionYear()
        {
            Value = default;
        }

        private FuelTransactionYear(int value)
        {
            Value = value;
        }

        public static FuelTransactionYear Create(int value)
        {
            return new FuelTransactionYear(value);
        }

        public static FuelTransactionYear Empty()
        {
            return new FuelTransactionYear();
        }
    }
}
