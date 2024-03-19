using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.AllocationEngine.Service
{
    public static class Dependencies
    {
        public static IServiceCollection AddCustomer(this IServiceCollection services)
        {
            //services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            //services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<AllocationService>();
            return services;
        }
    }
}
