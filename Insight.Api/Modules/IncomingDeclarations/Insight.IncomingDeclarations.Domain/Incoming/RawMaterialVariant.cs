using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class RawMaterialVariant : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterialVariant()
        {
            Value = string.Empty;
        }

        private RawMaterialVariant(string value)
        {
            Value = value;
        }

        public static RawMaterialVariant Create(string value)
        {
            return new RawMaterialVariant(value);
        }

        public static RawMaterialVariant Empty()
        {
            return new RawMaterialVariant();
        }
    }
}
