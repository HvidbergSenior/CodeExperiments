using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Fakes;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Serialization;
using Insight.Services.EmailSender.Service;
using Insight.Services.EmailSender.Service.Fakes;
using Insight.Services.EmailSender.Service.Fakes.Configuration;
using JasperFx.Core;
using Marten.PLv8;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oakton;
using Testcontainers.PostgreSql;
using Xunit;

namespace Insight.Tests.End2End
{
    public class WebAppFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithImage("clkao/postgres-plv8:11-2")
            .Build();

        public string ContainerId => postgreSqlContainer.Id;

        internal Guid UserId { get; private set; }
        internal WebApplicationFactory<Program> AppFactory { get; private set; }
        internal HttpClient CreateClient => AppFactory.CreateClient();

        public string ConnectionString => postgreSqlContainer.GetConnectionString();
        
        public WebAppFixture()
        {
            AppFactory = new();
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        }

        public IReadOnlyList<EmailMessage> GetAllEmailMessages()
        {
            return AppFactory.Services.GetService<IFakeEmailOutbox>()?.GetAllMessages() ?? new List<EmailMessage>();
        }

        public async Task InitializeAsync()
        {
            await postgreSqlContainer.StartAsync();

            UserId = Guid.NewGuid();

            OaktonEnvironment.AutoStartHost = true;

#pragma warning disable CA2000
            AppFactory = new WebAppFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var config = new MartenConfig();                    
                    config.ShouldRecreateDatabase = true;
                    config.SchemaName = "public";
                    config.ConnectionString = postgreSqlContainer.GetConnectionString();
                    config.AddFullTextExtension = true;
                    config.PopulateWithDemoData = true;
                    services.AddInsightMarten(config);
                    //services.AddSingleton<IExecutionContext>(new FakeExecutionContext(UserId));
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                    services.UseMockEmailSender();
                });
                builder.ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole());
            });
#pragma warning restore CA2000           

            foreach (var converter in JsonSerializerOptions().Converters)
            {
                SystemTextJsonSerializerConfig.Options.Converters.Add(converter);
            }
        }

        public Task DisposeAsync()
        {
            CreateClient?.Dispose();
            AppFactory?.Dispose();
            return postgreSqlContainer.DisposeAsync().AsTask();
        }

#pragma warning disable CA1822
        internal JsonSerializerOptions JsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            jsonOptions.Converters.Add(new SystemTextDateOnlyJsonConverter());
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            jsonOptions.Converters.Add(new SystemTextDateTimeOffsetJsonConverter());
            return jsonOptions;
        }
#pragma warning restore CA1822
    }
}
