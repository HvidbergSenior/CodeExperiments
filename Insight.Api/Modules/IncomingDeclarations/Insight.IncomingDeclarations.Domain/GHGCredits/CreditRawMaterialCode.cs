using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditRawMaterialCode : ValueObject
    {
        public string Value { get; private set; }

        private CreditRawMaterialCode()
        {
            Value = string.Empty;
        }

        private CreditRawMaterialCode(string value)
        {
            Value = value;
        }

        public static CreditRawMaterialCode Create(string value)
        {
            return new CreditRawMaterialCode(value);
        }

        public static CreditRawMaterialCode Empty()
        {
            return new CreditRawMaterialCode();
        }
    }
}