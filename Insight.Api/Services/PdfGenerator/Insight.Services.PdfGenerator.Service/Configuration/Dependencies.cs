using Insight.BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.PdfGenerator.Service.Configuration;

public static class Dependencies
{
    public static IServiceCollection UsePdfGeneratorService(this IServiceCollection services, IConfiguration config, bool isDevelopment )
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }
        
        var pdfGeneratorConfig = config.GetSection(PdfGeneratorConfig.DefaultConfigKey).Get<PdfGeneratorConfig>();
        if (pdfGeneratorConfig == null)
        {
            throw new MissingConfigurationException(nameof(pdfGeneratorConfig));
        }
        services.AddSingleton<IPdfGeneratorConfig>(pdfGeneratorConfig);
        services.AddScoped<IPdfGenerator, BrowserlessPdfGenerator>();
        
        return services;
    }
}