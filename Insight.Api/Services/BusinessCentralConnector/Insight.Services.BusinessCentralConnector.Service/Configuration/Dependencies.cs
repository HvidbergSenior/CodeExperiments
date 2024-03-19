using Insight.BuildingBlocks.Infrastructure;
using Insight.Services.BusinessCentralConnector.Service.Co2Target;
using Insight.Services.BusinessCentralConnector.Service.Company;
using Insight.Services.BusinessCentralConnector.Service.Configuration.LoadingDepots;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.FuelCardAcceptance;
using Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service.Itemledger;
using Insight.Services.BusinessCentralConnector.Service.Loadings;
using Insight.Services.BusinessCentralConnector.Service.Product;
using Insight.Services.BusinessCentralConnector.Service.RawMaterial;
using Insight.Services.BusinessCentralConnector.Service.Station;
using Insight.Services.BusinessCentralConnector.Service.TransactionsDialog;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.Services.BusinessCentralConnector.Service.Configuration;

public static class Dependencies
{
    public static IServiceCollection UseBusinessCentralService(this IServiceCollection services)
    {
        services.AddScoped<BusinessCentralCustomerService>();
        services.AddScoped<BusinessCentralFuelTransactionsTokheimService>();
        services.AddScoped<BusinessCentralCo2TargetService>();
        services.AddScoped<BusinessCentralRawMaterialService>();
        services.AddScoped<BusinessCentralFuelTransactionsTapnetService>();
        services.AddScoped<BusinessCentralFuelTransactionsDialogService>();
        services.AddScoped<BusinessCentralItemLedgerService>();
        services.AddScoped<BusinessCentralLoadingDepotService>();
        services.AddScoped<BusinessCentralLoadingService>();
        services.AddScoped<BusinessCentralStationService>();
        services.AddScoped<BusinessCentralProductService>();
        services.AddScoped<BusinessCentralCompanyService>();
        services.AddScoped<BusinessCentralFuelCardAcceptanceService>();
        services.AddScoped<BusinessCentralFuelCardBiofuelExpressService>();
        services.AddTransient<IBusinessCentralApiClient, BusinessCentralApiClient>();
        //services.AddSingleton<IDefaultDataProvider, CustomerDefaultDataProvider>();
        //services.AddSingleton<IDefaultDataProvider, CO2TargetDefaultDataProvider>();
        //services.AddSingleton<IDefaultDataProvider, AllowedRawMaterialDefaultDataProvider>();
        
        services.AddScoped<BusinessCentralUrlHelper>();
        services.AddScoped<BusinessCentralOptions>();
        services.AddScoped<BusinessCentralAuthHelper>();        
        return services;
    }

    public static IServiceCollection UseBusinessCentralOptions(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var businessCentralConfig = config.GetSection(BusinessCentralOptions.DefaultConfigKey).Get<BusinessCentralOptions>();
        if (businessCentralConfig == null)
        {
            throw new MissingConfigurationException(nameof(businessCentralConfig));
        }
        services.AddSingleton<IBusinessCentralConfig>(businessCentralConfig);
        return services;
    }
}