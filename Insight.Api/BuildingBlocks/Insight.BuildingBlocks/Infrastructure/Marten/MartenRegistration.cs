using System.Data;
﻿using Insight.BuildingBlocks.Configuration;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.BuildingBlocks.Serialization;
using Marten;
using Marten.PLv8;
using Marten.Schema;
using Marten.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Weasel.Core;
using Weasel.Postgresql;
using static System.Formats.Asn1.AsnWriter;

namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public static class MartenRegistration
    {
        public static IServiceCollection AddInsightMarten(this IServiceCollection services, IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var martenConfig = config.GetSection(MartenConfig.DefaultConfigKey).Get<MartenConfig>();
            if (martenConfig == null)
            {
                
                throw new MissingConfigurationException(nameof(martenConfig));
            }
            services.AddSingleton<IMartenConfig>(martenConfig);
            services.AddHealthChecks().AddNpgSql(martenConfig.ConnectionString);

            return AddInsightMartenWithConfig(services, martenConfig);
        }

        public static IServiceCollection AddInsightMarten(this IServiceCollection services, MartenConfig martenConfig)
        {
            if (martenConfig == null)
            {
                throw new ArgumentNullException(nameof(martenConfig));
            }
            return services.AddInsightMartenWithConfig(martenConfig);
        }

        private static IServiceCollection AddInsightMartenWithConfig(this IServiceCollection services, MartenConfig martenConfig)
        {
            var martenConfiguration = services.AddMarten(sp => {

                var options = new StoreOptions();                
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                options.UseJavascriptTransformsAndPatching();
                options.Logger(new MartenSerilogSink(sp));
                options.CreateDatabasesForTenants(c =>
                {
                    c.ForTenant()
                    .CheckAgainstPgDatabase()
                    .WithOwner("postgres")
                    .WithEncoding("UTF-8")
                    .ConnectionLimit(-1);
                });
                options.Connection(martenConfig.ConnectionString);
                options.DatabaseSchemaName = martenConfig.SchemaName;
                options.Serializer(GetJsonNetSerializer());

                if (martenConfig.AddFullTextExtension)
                {
                    options.Storage.ExtendedSchemaObjects.Add(new Extension("pg_trgm"));
                }
                return options;
            });

            if (martenConfig.PopulateWithDemoData)
            {
                martenConfiguration.InitializeWith<DataInitializer>();
            }

            if (martenConfig.AddFullTextExtension)
            {
                martenConfiguration.ApplyAllDatabaseChangesOnStartup();
            }

            return services;
        }

        public static JsonNetSerializer GetJsonNetSerializer()
        {
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters,
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.FloatParseHandling = FloatParseHandling.Decimal;
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                _.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                _.Converters.Add(new NewtonsoftDateOnlyJsonConverter());                
            });

            return serializer;
        }
    }
}