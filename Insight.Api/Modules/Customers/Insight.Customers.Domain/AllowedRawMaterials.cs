using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain
{
    public sealed class AllowedRawMaterials : ValueObject
    {
        public Dictionary<string,bool> Value { get; private set; } = new Dictionary<string, bool>();

        private AllowedRawMaterials()
        {
            Value = new Dictionary<string, bool>();
        }

        private AllowedRawMaterials(Dictionary<string, bool> value)
        {
            Value = value;
        }

        public static AllowedRawMaterials Create(Dictionary<string, bool> value)
        {
            return new AllowedRawMaterials(value);
        }

        public static AllowedRawMaterials Empty()
        {
            return new AllowedRawMaterials();
        }
    }
}
