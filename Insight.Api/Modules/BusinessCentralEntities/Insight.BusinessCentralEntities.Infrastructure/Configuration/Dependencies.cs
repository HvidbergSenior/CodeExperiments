using Insight.BuildingBlocks.Configuration;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.BusinessCentralEntities.Domain.Products;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Insight.BusinessCentralEntities.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddBusinessCentralEntities(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(Application.AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(Application.AssemblyReference.Assembly);
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            return services;
        }
    }

    public class ConfigureMarten : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema.For<Product>().UniqueIndex("unique_index_product_product", x => x.SourceSystemId.Value);
            options.Schema.For<Product>().Index(new Expression<Func<Product, object>>[] { x => x.ItemCategoryCode });
            options.Schema.For<Company>().UniqueIndex("unique_index_company_company", x => x.CompanyId.Value);
        }
    }
}
