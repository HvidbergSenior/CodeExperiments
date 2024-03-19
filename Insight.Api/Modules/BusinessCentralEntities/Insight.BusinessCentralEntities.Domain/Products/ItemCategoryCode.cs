using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain.Products
{
    public sealed class ItemCategoryCode : ValueObject
    {
        public string Value { get; private set; }

        private ItemCategoryCode()
        {
            Value = string.Empty;
        }

        private ItemCategoryCode(string value)
        {
            Value = value;
        }

        public static ItemCategoryCode Create(string value)
        {
            return new ItemCategoryCode(value);
        }

        public static ItemCategoryCode Empty()
        {
            return new ItemCategoryCode();
        }
    }
}
