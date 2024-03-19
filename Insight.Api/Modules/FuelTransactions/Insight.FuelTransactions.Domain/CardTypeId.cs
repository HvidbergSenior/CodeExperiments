using Insight.BuildingBlocks.Domain;

namespace Insight.FuelTransactions.Domain
{
    public sealed class CardTypeId : ValueObject
    {
        public string Value { get; private set; }

        private CardTypeId()
        {
            Value = string.Empty;
        }

        private CardTypeId(string value)
        {
            Value = value;
        }

        public static CardTypeId Create(string value)
        {
            return new CardTypeId(value);
        }

        public static CardTypeId Empty()
        {
            return new CardTypeId();
        }
    }
}