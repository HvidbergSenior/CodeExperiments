#pragma warning disable CA1506
using Insight.BuildingBlocks.Configuration;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Integration.Inbox.Configuration;
using Insight.BuildingBlocks.Integration.Outbox.Configuration;
using Insight.BuildingBlocks.Validation.WebApi.Middleware;
using Marten;
using Serilog;
using Microsoft.Extensions.Options;
using Insight.Services.BusinessCentralConnector.Service.Configuration;
using Insight.WebApplication.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using Insight.Customers.Infrastructure.Configuration;
using Insight.FuelTransactions.Infrastructure.Configuration;
using Insight.BusinessCentralEntities.Infrastructure.Configuration;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.TransactionsDialog;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim;
using Insight.BuildingBlocks.Infrastructure.Environment;
using Insight.BuildingBlocks.Infrastructure.Telemetry;
using Insight.Customers.Application;
using Insight.FuelTransactions.Application.GetOutgoingFuelTransactions;
using Insight.Stations.Infrastructure.Configuration;
using Insight.IncomingDeclarations.Application;
using Insight.IncomingDeclarations.Infrastructure.Configuration;
using Insight.OutgoingDeclarations.Application;
using Insight.OutgoingDeclarations.Infrastructure.Configuration;
using Insight.WebApplication.Services;
using Insight.PdfGenerator.Application.GeneratePdf;
using Insight.PdfGenerator.Infrastructure.Configuration;
using Insight.Services.AllocationEngine.Application;
using Insight.Services.PdfGenerator.Service.Configuration;
using Insight.UserAccess.Infrastructure.Configuration;
using Insight.UserAccess.Application;
using Insight.Services.AllocationEngine.Application.Configuration;
using Insight.Services.AllocationEngine.Infrastructure.Configuration;
using Oakton;
using Insight.FuelTransactions.Application.GetStockTransactions;
using Insight.FuelTransactions.Application.CreateStockTransaction;
using Insight.Services.BusinessCentralConnector.Service.Itemledger;
using Insight.Services.EmailSender.Service.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration)
           .Enrich.FromLogContext()
           .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
           .WriteTo.ApplicationInsights(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"], TelemetryConverter.Traces)
           .WriteTo.Seq("http://localhost:5341")
           .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEnvironment();
builder.Services.AddTelemetry(builder.Configuration, builder.Environment.IsProduction());


builder.Services.AddAuth();

// Add services to the container.
builder.Services.AddOutbox();
builder.Services.AddInbox();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInsightSwaggerGen();
builder.Services.UseBuildingBlocks();
builder.Services.AddInsightMarten(builder.Configuration);
builder.Services.AddScoped<IDocumentSession>(_ => _.GetRequiredService<IDocumentStore>().IdentitySession(new Marten.Services.SessionOptions() { Timeout = 300 }));
builder.Services.AddScoped<IEntityEventsPublisher, EntityEventsPublisher>();
builder.Services.AddScoped<IUnitOfWork, MartenUnitOfWork>();

builder.Services.AddHttpClient();
builder.Services.UseBusinessCentralService();
builder.Services.UseBusinessCentralOptions(builder.Configuration);
builder.Services.UsePdfGeneratorService(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.UseSmtpEmailSender(builder.Configuration);
builder.Services.AddPdfGenerator();
builder.Services.AddCustomer();
builder.Services.AddBusinessCentralEntities();
builder.Services.AddIncomingDeclaration();
builder.Services.AddFuelTransactions();
builder.Services.AddStations();
builder.Services.AddOutgoingDeclaration();
builder.Services.AddAllocation();
builder.Services.AddAllocationDraftRepository();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHostedService<BusinessCentralCustomerUpdateService>();
builder.Services.AddHostedService<EntityCustomerRelationUpdateService>();
builder.Services.AddHostedService<ProductsFromBCUpdater>();
builder.Services.AddHostedService<CompaniesFromBCUpdater>();
builder.Services.AddHostedService<BusinessCentralFuelTransactionsTokheimUpdateService>();
builder.Services.AddHostedService<BusinessCentralFuelTransactionsTapnetUpdateService>();
builder.Services.AddHostedService<BusinessCentralFuelTransactionsDialogUpdateService>();
builder.Services.AddHostedService<BusinessCentralItemLedgerUpdateService>();

builder.Services.AddMemoryCache();

builder.Services.AddHealthChecks()
    .AddApplicationInsightsPublisher();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHsts();

app.UseTraceIds();
app.UseSerilogRequestLogging();

// Todo: Do not do this in production.
app.UseMiddleware<ExceptionHandlerMiddleware>(Options.Create(new ExceptionHandlerOption { ShowCallStackInHttpResponse = true }));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health").AllowAnonymous();
app.MapControllers();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

// 
app.MapUserAccessEndpoints();
app.MapGetIncomingDeclarationsEndpoints();
app.MapGetOutgoingFuelTransactionsEndpoint();
app.MapGeneratePdfEndpoint();
app.MapOutgoingDeclarationsEndpoints();
app.MapCustomerEndpoints();
app.MapAllocationEndpoints();
app.MapGetStockTransactionsEndpoint();
app.MapCreateStockTransactionEndpoint();


app.UseCors(
    opt =>
    {
        opt.AllowAnyOrigin();
        opt.AllowAnyHeader();
        opt.AllowAnyMethod();
        opt.WithExposedHeaders("X-Trace-Id");
    }
);
return await app.RunOaktonCommands(args);
#pragma warning restore CA1506