using System.Linq.Expressions;
using Insight.BuildingBlocks.Configuration;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.IncomingDeclarations.Application;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Domain.Parsing;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Parser;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.IncomingDeclarations.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddIncomingDeclaration(this IServiceCollection services)
        {
            services.AddMediatorFromAssembly(AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(AssemblyReference.Assembly);
            services.AddScoped<IIncomingDeclarationRepository, IncomingDeclarationRepository>();
            services.AddScoped<IRawMaterialTranslationRepository, RawMaterialTranslationRepository>();
            services.AddScoped<IProductTranslationRepository, ProductTranslationRepository>();
            services.AddScoped<IIncomingDeclarationAllocationRepository, IncomingDeclarationAllocationRepository>();
            services.AddTransient<IIncomingDeclarationParser, BFEFormatParser>();
            services.AddTransient<IIncomingDeclarationParser, NesteFormatParserExcelDataReader>();
            services.AddTransient<IRawMaterialTranslationRepository, RawMaterialTranslationRepository>();
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            services.AddSingleton<IDefaultDataProvider, DefaultRawMaterialTranslationProvider>();
            services.AddSingleton<IDefaultDataProvider, DefaultProductTranslationProvider>();

            return services;
        }

        public class ConfigureMarten : IConfigureMarten
        {
            public void Configure(IServiceProvider services, StoreOptions options)
            {
                options.Schema.For<IncomingDeclaration>().UniqueIndex("unique_index_incomingdeclaration_incomingdeclaration", x => x.IncomingDeclarationId.Value);
                options.Schema.For<IncomingDeclaration>().UniqueIndex("unique_index_incomingdeclaration_hash", x => x.ItemHash);
                options.Schema.For<IncomingDeclaration>().Index(new Expression<Func<IncomingDeclaration, object>>[] { x => x.IncomingDeclarationState, x => x.IncomingDeclarationUploadId.Value },c=> c.Name = "mt_doc_incomingdeclaration_idx_stateincoming_upload_id_value");
            }
        }
    }
}
