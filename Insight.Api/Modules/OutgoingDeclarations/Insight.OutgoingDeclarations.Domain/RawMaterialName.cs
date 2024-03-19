using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class RawMaterialName : ValueObject
    {
        public string Value { get; private set; }

        private RawMaterialName()
        {
            Value = string.Empty;
        }

        private RawMaterialName(string value)
        {
            Value = value;
        }

        public static RawMaterialName Create(string company)
        {
            return new RawMaterialName(company);
        }

        public static RawMaterialName Empty()
        {
            return new RawMaterialName();
        }
    }
}