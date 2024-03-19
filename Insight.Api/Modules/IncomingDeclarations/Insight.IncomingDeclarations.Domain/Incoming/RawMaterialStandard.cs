using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class RawMaterialStandard : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterialStandard()
        {
            Value = string.Empty;
        }

        private RawMaterialStandard(string value)
        {
            Value = value;
        }

        public static RawMaterialStandard Create(string value)
        {
            return new RawMaterialStandard(value);
        }

        public static RawMaterialStandard Empty()
        {
            return new RawMaterialStandard();
        }
    }
}
