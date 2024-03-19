using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class RawMaterial : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterial()
        {
            Value = string.Empty;
        }

        private RawMaterial(string value)
        {
            Value = value;
        }

        public static RawMaterial Create(string value)
        {
            return new RawMaterial(value);
        }

        public static RawMaterial Empty()
        {
            return new RawMaterial();
        }
    }
}