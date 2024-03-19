using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Insight.BuildingBlocks.Configuration
{
    public static class MediatorRegistrar
    {
        public static IServiceCollection AddMediatorFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
