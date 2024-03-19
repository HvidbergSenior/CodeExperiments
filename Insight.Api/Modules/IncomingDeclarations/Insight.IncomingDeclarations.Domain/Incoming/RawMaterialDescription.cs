using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class RawMaterialDescription : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterialDescription()
        {
            Value = string.Empty;
        }

        private RawMaterialDescription(string value)
        {
            Value = value;
        }

        public static RawMaterialDescription Create(string value)
        {
            return new RawMaterialDescription(value);
        }

        public static RawMaterialDescription Empty()
        {
            return new RawMaterialDescription();
        }
    }
}
