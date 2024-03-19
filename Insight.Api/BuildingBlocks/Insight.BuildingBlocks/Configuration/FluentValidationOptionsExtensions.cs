using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Insight.BuildingBlocks.Configuration
{
    public static class FluentValidationOptionsExtensions
    {
        public static OptionsBuilder<TOptions> AddWithValidation<TOptions, TValidator>(
            this IServiceCollection services,
            string configurationSection)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
        {
            // Add the validator
            services.AddScoped<IValidator<TOptions>, TValidator>();

            return services.AddOptions<TOptions>()
                .BindConfiguration(configurationSection)
                .ValidateFluentValidation()
                .ValidateOnStart();
        }
    }
}
