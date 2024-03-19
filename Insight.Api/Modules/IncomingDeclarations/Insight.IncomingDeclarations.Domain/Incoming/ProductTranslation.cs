using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ProductTranslation : Entity
    {
        public ProductStandard ProductStandard { get; private set; }
        public ProductDescription ProductDescription { get; private set; }

        public List<ProductVariant> ProductVariants { get; private set; }

        private ProductTranslation()
        {
            Id = Guid.Empty;
            ProductStandard = ProductStandard.Empty();
            ProductDescription = ProductDescription.Empty();
            ProductVariants = new List<ProductVariant>();
        }

        private ProductTranslation(ProductStandard productStandard,
            ProductDescription productDescription,
            List<ProductVariant> productVariantList)
        {
            Id = Guid.NewGuid();
            ProductStandard = productStandard;
            ProductDescription = productDescription;
            ProductVariants = productVariantList;
        }

        public static ProductTranslation Create(
            ProductStandard ProductStandard,
            ProductDescription ProductDescription,
            ProductVariant ProductVariant)
        {
            return new ProductTranslation(
                ProductStandard,
                ProductDescription,
                new List<ProductVariant>() { ProductVariant });
        }

        public static ProductTranslation Create(
            ProductStandard ProductStandard,
            ProductDescription ProductDescription,
            List<ProductVariant> ProductVariantList)
        {
            return new ProductTranslation(
                ProductStandard,
                ProductDescription,
                ProductVariantList);
        }
    }
}
