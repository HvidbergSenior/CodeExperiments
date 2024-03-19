using System.Linq.Expressions;
using Insight.BuildingBlocks.Configuration;
using Insight.Customers.Application;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.Customers.Infrastructure.Repositories;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Customers.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddCustomer(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerHierarchy, CustomerHierarchy>();
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());

            return services;
        }
    }

    public class ConfigureMarten : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema.For<Customers.Domain.Customer>().UniqueIndex("unique_index_customer_customer", x => x.CustomerId.Value);
            options.Schema.For<Customers.Domain.Customer>().Index(new Expression<Func<Domain.Customer, object>>[] { x => x.NumberNumber.Value, x => x.CompanyId.Value });
        }
    }
}
