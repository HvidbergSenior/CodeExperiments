using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.Services.AllocationEngine.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.AllocationEngine.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddAllocationDraftRepository(this IServiceCollection services)
        {
            services.AddSingleton<IDefaultDataProvider, DefaultAllocationDraftSeeder>();
            services.AddScoped<IAllocationDraftRepository, AllocationDraftRepository>();  
            services.AddTransient<ISequenceBatchIdGenerator, SequenceBatchIdGenerator>();

            return services;
        }
    }
}
