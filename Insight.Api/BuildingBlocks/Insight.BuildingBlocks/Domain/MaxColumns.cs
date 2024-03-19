namespace Insight.BuildingBlocks.Domain
{
    public sealed class MaxColumns : ValueObject
    {
        public int Value { get; private set; }

        private MaxColumns()
        {
            Value = default;
        }

        private MaxColumns(int value)
        {
            Value = value;
        }

        public static MaxColumns Create(int value)
        {
            return new MaxColumns(value);
        }

        public static MaxColumns Empty()
        {
            return new MaxColumns();
        }
    }
}