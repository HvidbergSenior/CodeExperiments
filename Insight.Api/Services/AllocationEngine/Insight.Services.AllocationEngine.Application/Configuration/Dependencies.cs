using Insight.BuildingBlocks.Configuration;
using Insight.Services.AllocationEngine.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.AllocationEngine.Application.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddAllocation(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<AllocationService>();
            return services;
        }
    }
}
