using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain.SustainabilityReports
{
    public sealed class FuelTransactionMonth : ValueObject
    {
        public int Value { get; private set; }

        private FuelTransactionMonth()
        {
            Value = default;
        }

        private FuelTransactionMonth(int value)
        {
            Value = value;
        }

        public static FuelTransactionMonth Create(int value)
        {
            return new FuelTransactionMonth(value);
        }

        public static FuelTransactionMonth Empty()
        {
            return new FuelTransactionMonth();
        }
    }
}
