using System.Diagnostics;
using System.Diagnostics.Metrics;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Insight.BuildingBlocks.Telemetry;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Insight.BuildingBlocks.Infrastructure.Telemetry;

// Todo: The OpenTelemetry still needs some work with regards to deployed Azure Resources and configuration/implementation in this project.
public static class TelemetryRegistration
{

    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration config, bool isProduction)
    {
        if (!isProduction)
        {
            return services;
        }
        
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        var applicationInsightsConfig = config.GetSection(ApplicationInsightsConfig.DefaultConfigKey).Get<ApplicationInsightsConfig>();
        if (applicationInsightsConfig == null)
        {
            throw new MissingConfigurationException(nameof(applicationInsightsConfig));
        }
        services.AddSingleton<IApplicationInsightsConfig>(applicationInsightsConfig);
        
        var telemetryConfig = config.GetSection(TelemetryConfig.DefaultConfigKey).Get<TelemetryConfig>();
        if (telemetryConfig == null)
        {
            throw new MissingConfigurationException(nameof(telemetryConfig));
        }
        services.AddSingleton<ITelemetryConfig>(telemetryConfig);
        
        return AddTelemetryWithConfig(services,applicationInsightsConfig, telemetryConfig);
    }


    private static IServiceCollection AddTelemetryWithConfig(this IServiceCollection services,
        IApplicationInsightsConfig applicationInsightsConfig, ITelemetryConfig telemetryConfig )
    {
        if (string.IsNullOrEmpty(applicationInsightsConfig.ConnectionString) )
        {
            return services;
        }

        services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions()
        {
            ConnectionString = applicationInsightsConfig.ConnectionString
            //EnableAdaptiveSampling = !bool.TryParse(builder.Configuration["APPLICATIONINSIGHTS_ADAPTIVE_SAMPLING"], out var result) || result
        });
        
        // Open Telemetry
        Meter meter = new(telemetryConfig.ServiceName);
        ActivitySource activitySource = new(telemetryConfig.ServiceName);
        services.ConfigureOpenTelemetryMeterProvider((sp, builder) => {
            builder.AddMeter(meter.Name);
        });


        //builder.AddSeq(); //

        return services.AddOpenTelemetry()
            .ConfigureResource(c=> c.AddService(telemetryConfig.ServiceName))
            .UseAzureMonitor(c => c.ConnectionString = applicationInsightsConfig.ConnectionString)
            .WithMetrics(c=> c.AddMeter(meter.Name)
                .AddOtlpExporter())
            .WithTracing(c=> 
            {
                c.AddOtlpExporter();
                c.AddAspNetCoreInstrumentation();
                c.AddHttpClientInstrumentation();
            }).Services;
    }

}