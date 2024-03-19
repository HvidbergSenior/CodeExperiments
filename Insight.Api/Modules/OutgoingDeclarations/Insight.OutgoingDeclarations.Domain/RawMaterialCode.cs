using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class RawMaterialCode : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterialCode()
        {
            Value = string.Empty;
        }

        private RawMaterialCode(string value)
        {
            Value = value;
        }

        public static RawMaterialCode Create(string company)
        {
            return new RawMaterialCode(company);
        }

        public static RawMaterialCode Empty()
        {
            return new RawMaterialCode();
        }
    }
}