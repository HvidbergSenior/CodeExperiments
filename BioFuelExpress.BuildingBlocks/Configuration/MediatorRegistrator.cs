using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BioFuelExpress.BuildingBlocks.Configuration
{
    public static class MediatorRegistrator
    {
        public static IServiceCollection AddMediatorFromAssembly(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
         
            return services;
        }
    }
}
