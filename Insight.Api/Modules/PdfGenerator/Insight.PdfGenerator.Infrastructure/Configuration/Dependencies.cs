using Insight.BuildingBlocks.Configuration;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.PdfGenerator.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddPdfGenerator(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(Application.AssemblyReference.Assembly);
            return services;
        }
    }

}
