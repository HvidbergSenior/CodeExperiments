using Insight.BuildingBlocks.Configuration;
using Insight.OutgoingDeclarations.Domain;
using Microsoft.Extensions.DependencyInjection;
using AssemblyReference = Insight.OutgoingDeclarations.Application.AssemblyReference;

namespace Insight.OutgoingDeclarations.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddOutgoingDeclaration(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<IOutgoingDeclarationRepository, OutgoingDeclarationRepository>();

            return services;
        }
    }
}
