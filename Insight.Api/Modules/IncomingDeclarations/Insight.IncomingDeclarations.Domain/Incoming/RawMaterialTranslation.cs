using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class RawMaterialTranslation : Entity
    {
        public RawMaterialStandard RawMaterialStandard { get; private set; }
        public RawMaterialDescription RawMaterialDescription { get; private set; }

        public List<RawMaterialVariant> RawMaterialVariants { get; private set; }

        private RawMaterialTranslation()
        {
            Id = Guid.Empty;
            RawMaterialStandard = RawMaterialStandard.Empty();
            RawMaterialDescription = RawMaterialDescription.Empty();
            RawMaterialVariants = new List<RawMaterialVariant>();
        }

        private RawMaterialTranslation(
            RawMaterialStandard rawMaterialStandard,
            RawMaterialDescription rawMaterialDescription,
            List<RawMaterialVariant> rawMaterialVariantList)
        {
            Id = Guid.NewGuid();
            RawMaterialStandard = rawMaterialStandard;
            RawMaterialDescription = rawMaterialDescription;
            RawMaterialVariants = rawMaterialVariantList;
        }

        public static RawMaterialTranslation Create(
            RawMaterialStandard rawMaterialStandard,
            RawMaterialDescription rawMaterialDescription,
            RawMaterialVariant rawMaterialVariant)
        {
            return new RawMaterialTranslation(
                rawMaterialStandard, 
                rawMaterialDescription,
                new List<RawMaterialVariant>() {rawMaterialVariant});
        }

        public static RawMaterialTranslation Create(
            RawMaterialStandard rawMaterialStandard,
            RawMaterialDescription rawMaterialDescription,
            List<RawMaterialVariant> rawMaterialVariantList)
        {
            return new RawMaterialTranslation(
                rawMaterialStandard,
                rawMaterialDescription,
                rawMaterialVariantList);
        }
    }
}