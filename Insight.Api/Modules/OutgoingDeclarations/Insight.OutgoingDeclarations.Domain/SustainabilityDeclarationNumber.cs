using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class SustainabilityDeclarationNumber : ValueObject
    {
        public string Value { get; private set; }

        private SustainabilityDeclarationNumber()
        {
            Value = string.Empty;
        }

        private SustainabilityDeclarationNumber(string value)
        {
            Value = value;
        }

        public static SustainabilityDeclarationNumber Create(string value)
        {
            return new SustainabilityDeclarationNumber(value);
        }

        public static SustainabilityDeclarationNumber Empty()
        {
            return new SustainabilityDeclarationNumber();
        }
    }
}