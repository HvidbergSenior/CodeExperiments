using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.Services.BusinessCentralConnector.Service;
using Insight.Services.BusinessCentralConnector.Service.Company;
using Insight.Services.BusinessCentralConnector.Service.Product;

namespace Insight.WebApplication.Services
{
    public static class BusinessCentralExtensions
    {
        public static Product ToProduct(this BusinessCentralProduct product)
        {
            var sourceSystemId = SourceSystemId.Create(product.SystemId);
            var itemCategoryCode = ItemCategoryCode.Create(product.ItemCategoryCode!);
            var productNumber = ProductNumber.Create(product.Number!);
            var description = Description.Create(product.Description!);
            var sourceSystemEtag = SourcesystemEtag.Create(product.Etag!);
            var companyId = CompanyId.Create(product.CompanyId);
            var companyName = CompanyName.Create(product.CompanyName);
            return Product.Create(sourceSystemId, itemCategoryCode, productNumber, description, sourceSystemEtag, companyId, companyName);
        }

        public static Company ToCompany(this BusinessCentralCompany company)
        {
            var companyId = CompanyId.Create(company.Id);
            var companyName = CompanyName.Create(company.Name);
            var sourceSystemEtag = SourcesystemEtag.Create(company.SystemVersion!);
            return Company.Create(companyId, companyName, sourceSystemEtag);
        }
    }
}
