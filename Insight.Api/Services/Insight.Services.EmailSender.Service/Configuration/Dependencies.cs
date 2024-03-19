using Insight.BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.EmailSender.Service.Configuration;

public static class Dependencies
{
    public static IServiceCollection UseSmtpEmailSender(this IServiceCollection services, IConfiguration config)
    {
        var emailConfiguration = config.GetSection(IEmailConfiguration.DefaultConfigKey).Get<EmailConfiguration>();
        if (emailConfiguration == null)
        {
            throw new MissingConfigurationException(nameof(emailConfiguration));
        }

        if (emailConfiguration.UseEmailConfiguration)
        {
            if (string.IsNullOrEmpty(emailConfiguration.Username))
            {
                throw new MissingConfigurationException(nameof(emailConfiguration) + " missing Username");
            }
            if (string.IsNullOrEmpty(emailConfiguration.SmtpServer))
            {
                throw new MissingConfigurationException(nameof(emailConfiguration) + " missing SmtpServer");
            }
            if (string.IsNullOrEmpty(emailConfiguration.Password))
            {
                throw new MissingConfigurationException(nameof(emailConfiguration) + " missing Password");
            }

            services.AddSingleton<IEmailConfiguration>(emailConfiguration);
            services.AddScoped<IEmailSender, SmtpEmailSender>();
        }

        return services;
    }
}