using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Insight.WebApplication.Configuration
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddInsightSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Insight API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                        {
                                            Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                        }
                                },
                                Array.Empty<string>()
                        }
                    });
                option.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Example = new OpenApiString("2024-01-01")
                });
            });
            return services;
        }
    }
}
