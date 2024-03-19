using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditRawMaterialDescription : ValueObject
    {
        public string Value { get; private set; }

        private CreditRawMaterialDescription()
        {
            Value = string.Empty;
        }

        private CreditRawMaterialDescription(string value)
        {
            Value = value;
        }

        public static CreditRawMaterialDescription Create(string value)
        {
            return new CreditRawMaterialDescription(value);
        }

        public static CreditRawMaterialDescription Empty()
        {
            return new CreditRawMaterialDescription();
        }
    }
}