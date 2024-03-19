using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.EmailSender.Service.Fakes.Configuration;

public static class Dependencies
{
    public static IServiceCollection UseMockEmailSender(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, FakeEmailSender>();
        services.AddSingleton<IFakeEmailOutbox, FakeEmailOutbox>();

        return services;
    }
}