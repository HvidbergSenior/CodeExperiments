using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionsOffset : Entity
    {
        public FuelTransactionsOffsetId FuelTransactionsOffsetId { get; private set; }
        public FuelTransactionPosSystem FuelTransactionPosSystem { get; private set; }
        public FuelTransactionsOffsetTime FuelTransactionsOffsetTime { get; private set; }
        public int EntriesToSkip { get; private set; }

        private FuelTransactionsOffset()
        {
            FuelTransactionsOffsetId = FuelTransactionsOffsetId.Empty();
            Id = FuelTransactionsOffsetId.Value;
            FuelTransactionsOffsetTime = FuelTransactionsOffsetTime.Empty();
        }

        private FuelTransactionsOffset(FuelTransactionsOffsetId fuelTransactionsOffsetId, FuelTransactionPosSystem fuelTransactionPosSystem, FuelTransactionsOffsetTime fuelTransactionsOffsetTime)
        {
            Id = fuelTransactionsOffsetId.Value;
            FuelTransactionsOffsetId = fuelTransactionsOffsetId;
            FuelTransactionPosSystem = fuelTransactionPosSystem;
            FuelTransactionsOffsetTime = fuelTransactionsOffsetTime;
        }

        public static FuelTransactionsOffset Create(FuelTransactionsOffsetId fuelTransactionsOffsetId, FuelTransactionPosSystem fuelTransactionPosSystem, FuelTransactionsOffsetTime fuelTransactionsOffsetTime)
        {
            return new FuelTransactionsOffset(fuelTransactionsOffsetId, fuelTransactionPosSystem, fuelTransactionsOffsetTime);
        }

        public static FuelTransactionsOffset Empty()
        {
            return new FuelTransactionsOffset();
        }

        public void SetFuelTransactionsOffsetTime(FuelTransactionsOffsetTime fuelTransactionsOffsetTime)
        {
            FuelTransactionsOffsetTime = fuelTransactionsOffsetTime;
        }

        public void SetEntriesToSkip(int entriesToSkip)
        {
            EntriesToSkip = entriesToSkip;
        }
    }
}