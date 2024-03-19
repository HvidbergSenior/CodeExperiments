using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain.Products
{
    public sealed class Product : Entity
    {
        public SourceSystemId SourceSystemId { get; private set; } = SourceSystemId.Empty();
        public ItemCategoryCode ItemCategoryCode { get; private set; } = ItemCategoryCode.Empty();
        public ProductNumber ProductNumber { get; private set; } = ProductNumber.Empty();
        public Description Description { get; private set; } = Description.Empty();
        public CompanyId CompanyId { get; private set; } = CompanyId.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();
        public SourcesystemEtag SourcesystemEtag { get; private set; } = SourcesystemEtag.Empty();

        private Product()
        {
            // Intentinally left blank
        }

        private Product(SourceSystemId sourceSystemId, ItemCategoryCode itemCategoryCode, ProductNumber productNumber, Description description, SourcesystemEtag sourcesystemEtag, CompanyId companyId, CompanyName companyName)
        {
            SourceSystemId = sourceSystemId;
            ItemCategoryCode = itemCategoryCode;
            ProductNumber = productNumber;
            Description = description;
            SourcesystemEtag = sourcesystemEtag;
            CompanyId = companyId;
            CompanyName = companyName;
        }

        public void Update(ItemCategoryCode itemCategoryCode, ProductNumber productNumber, Description description, SourcesystemEtag sourcesystemEtag)
        {
            SourcesystemEtag = sourcesystemEtag;
            ItemCategoryCode = itemCategoryCode;
            ProductNumber = productNumber;
            Description = description;
        }

        public static Product Create(SourceSystemId sourceSystemId, ItemCategoryCode itemCategoryCode, ProductNumber productNumber, Description description, SourcesystemEtag sourcesystemEtag, CompanyId companyId, CompanyName companyName)
        {
            return new Product(sourceSystemId, itemCategoryCode, productNumber, description, sourcesystemEtag, companyId, companyName);
        }
    }
}
