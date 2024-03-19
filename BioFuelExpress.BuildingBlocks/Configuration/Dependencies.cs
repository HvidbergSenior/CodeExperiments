using BioFuelExpress.BuildingBlocks.Application.Commands;
using BioFuelExpress.BuildingBlocks.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace BioFuelExpress.BuildingBlocks.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UseBuildingBlocks(this IServiceCollection services)
        {
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();

            return services;
        }
    }
}
