using Insight.BuildingBlocks.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Marten;
using Insight.Stations.Domain;

namespace Insight.Stations.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddStations(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(Application.AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(Application.AssemblyReference.Assembly);
            services.AddScoped<IStationRepository, StationRepository>();            
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());

            return services;
        }

        public class ConfigureMarten : IConfigureMarten
        {
            public void Configure(IServiceProvider services, StoreOptions options)
            {
                options.Schema.For<Station>().UniqueIndex("unique_index_station_station", x => x.StationId.Value);
                options.Schema.For<Station>().Index(c => c.StationNumber.Value);
            }
        }

    }
}
