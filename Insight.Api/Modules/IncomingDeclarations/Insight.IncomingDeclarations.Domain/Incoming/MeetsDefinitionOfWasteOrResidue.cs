using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class MeetsDefinitionOfWasteOrResidue : ValueObject
    {
        public bool Value { get; private set; }

        private MeetsDefinitionOfWasteOrResidue()
        {
            Value = false;
        }

        private MeetsDefinitionOfWasteOrResidue(bool value)
        {
            Value = value;
        }

        public static MeetsDefinitionOfWasteOrResidue Create(bool value)
        {
            return new MeetsDefinitionOfWasteOrResidue(value);
        }

        public static MeetsDefinitionOfWasteOrResidue None()
        {
            return new MeetsDefinitionOfWasteOrResidue();
        }
    }
}
