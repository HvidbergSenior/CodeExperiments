using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class CardAcceptanceId : ValueObject
    {
        public string Value { get; private set; }

        private CardAcceptanceId()
        {
            Value = string.Empty;
        }

        private CardAcceptanceId(string value)
        {
            Value = value;
        }

        public static CardAcceptanceId Create(string value)
        {
            return new CardAcceptanceId(value);
        }

        public static CardAcceptanceId Empty()
        {
            return new CardAcceptanceId();
        }
    }
}