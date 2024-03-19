namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public class RawMaterialTranslations
    {
        public static RawMaterial TranslateOrDefault(IReadOnlyList<RawMaterialTranslation> rawMaterialTranslations, string rawMaterialValue)
        {
            //Case sensitiv: var rawMaterialValue = rawMaterialTranslations.FirstOrDefault(c => c.RawMaterialVariants.Contains(RawMaterialVariant.Create(RawMaterial)))?.RawMaterialStandard.Value;
            var rawMaterialValidValue = rawMaterialTranslations.FirstOrDefault(
                c => c.RawMaterialVariants.Any(
                    desc => StringComparer.OrdinalIgnoreCase.Equals(desc.Value, rawMaterialValue)))?
                    .RawMaterialStandard.Value;

            RawMaterial rawMaterial = !string.IsNullOrWhiteSpace(rawMaterialValidValue) ?
                RawMaterial.Create(rawMaterialValidValue) : RawMaterial.Create(rawMaterialValue);

            return rawMaterial;
        }
    }
}
