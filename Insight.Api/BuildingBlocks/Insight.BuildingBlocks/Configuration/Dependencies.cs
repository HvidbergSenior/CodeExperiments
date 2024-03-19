using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.PipelineBehaviour;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insight.BuildingBlocks.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection UseBuildingBlocks(this IServiceCollection services)
        {
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();
            services.AddTransient<HttpContextAccessor>();
            //services.AddScoped<IExecutionContext, DefaultExecutionContext>(); // TODO if this is correct, when do we inject AuthenticatedExecutionContext ?
            services.AddScoped<IEntityEventsPublisher, EntityEventsPublisher>();
            services.AddTransient<IUnitOfWork, MartenUnitOfWork>();

            //services.AddSingleton<IIdGenerator<Guid>, SequentialGuidIdGenerator>();

            //services.AddSingleton<IEnvironment, Environment>();
            //services.AddSingleton<ISystemTime, SystemTime>();

            // Order of the pipeline registration is important.
            // Pipeline will be executed in the registered order.

            // Validation of the command / query is the first step in our pipeline
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Loggingbehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Validationbehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            //
            services.TryAddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

            return services;
        }
    }
}
