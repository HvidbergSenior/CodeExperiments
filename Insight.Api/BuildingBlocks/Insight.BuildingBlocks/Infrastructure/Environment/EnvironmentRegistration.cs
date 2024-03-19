using Microsoft.Extensions.DependencyInjection;

namespace Insight.BuildingBlocks.Infrastructure.Environment;

public static class EnvironmentRegistration
{
    public static IServiceCollection AddEnvironment(this IServiceCollection services)
    {
        return services.AddSingleton<IEnvironment, Environment>();
    }

}