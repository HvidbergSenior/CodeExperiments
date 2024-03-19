using Dapper;
using Insight.BuildingBlocks.Configuration;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Insight.FuelTransactions.Infrastructure.OutgoingTransactions;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.FuelTransactions.Infrastructure.Allocation;
using Insight.FuelTransactions.Domain.Stock;

namespace Insight.FuelTransactions.Infrastructure.Configuration
{
    public static class Dependencies
    {
        public static IServiceCollection AddFuelTransactions(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());

            services.AddMediatorFromAssembly(Application.AssemblyReference.Assembly);
            services.AddAuthorizersFromAssembly(Application.AssemblyReference.Assembly);
            services.AddScoped<IFuelTransactionsRepository, FuelTransactionsRepository>();
            services.AddScoped<IFuelTransactionsTimeStampOffsetRepository, FuelTransactionsTimeStampOffsetRepository>();
            services.AddSingleton<IConfigureMarten>(new ConfigureMarten());
            services.AddSingleton<IDefaultDataProvider, DefaultFuelTransactionDataProvider>();
            services.AddScoped<IOutgoingFuelTransactionsRepository, OutgoingFuelTransactionsRepository>();
            services.AddScoped<IFuelTransactionsAllocationRepository, FuelTransactionsAllocationRepository>();
            services.AddScoped<IStockTransactionsRepository, StockTransactionsRepository>();
            return services;
        }
    }

    public class ConfigureMarten : IConfigureMarten
    {
        public void Configure(IServiceProvider services, StoreOptions options)
        {
            options.Schema.For<FuelTransaction>().UniqueIndex("unique_index_fueltransaction_hash", x => x.ItemHash);
            options.Schema.For<FuelTransaction>().Index(c => c.DraftAllocationId.Value);
            options.Schema.For<FuelTransaction>().Index(new Expression<Func<FuelTransaction, object>>[] { x => x.CustomerNumber.Value, x => x.ProductNumber.Value, x => x.StationNumber.Value }, c=> c.Name = "index_fueltransaction_customer_product_station");
            options.Schema.For<FuelTransaction>().Index(new Expression<Func<FuelTransaction, object>>[] { x => x.CustomerNumber.Value, x => x.ProductNumber.Value, x => x.LocationId }, c=> c.Name = "index_fueltransaction_customer_product_location");
            options.Schema.For<FuelTransaction>().Duplicate(x => x.FuelTransactionTimeStamp);

            options.Schema.For<StockTransaction>().UniqueIndex("unique_index_stocktransaction_hash", x => x.ItemHash);
            options.Schema.For<StockTransaction>().Index(new Expression<Func<StockTransaction, object>>[] { x => x.Location.Value, x => x.ProductName.Value, x => x.Country.Value }, c => c.Name = "index_stocktransaction_location_product_country");
            options.Schema.For<StockTransaction>().Duplicate(x=> x.TransactionDate.Value);
        }
    }
}
