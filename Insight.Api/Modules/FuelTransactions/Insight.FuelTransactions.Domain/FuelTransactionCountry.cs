namespace Insight.FuelTransactions.Domain
{
    public sealed class FuelTransactionCountry
    {
        public string Value { get; private set; }

        private FuelTransactionCountry()
        {
            Value = string.Empty;
        }

        private FuelTransactionCountry(string value)
        {
            Value = value;
        }

        public static FuelTransactionCountry Create(string value)
        {
            return new FuelTransactionCountry(value);
        }

        public static FuelTransactionCountry Empty()
        {
            return new FuelTransactionCountry();
        }   
    }
}
