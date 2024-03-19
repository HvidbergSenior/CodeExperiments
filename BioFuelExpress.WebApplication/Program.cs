using BioFuelExpress.BuildingBlocks.Configuration;
using BioFuelExpress.BuildingBlocks.Events;
using BioFuelExpress.BuildingBlocks.Infrastructure;
using BioFuelExpress.BuildingBlocks.Infrastructure.Marten;
using BioFuelExpress.Domain;
using BioFuelExpress.Infrastructure;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(BioFuelExpress.Application.AssemblyReference.Assembly));

builder.Services.UseBuildingBlocks();

builder.Services.AddSingleton<IDocumentStore>(_ =>
{
    var connectionString = "Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=GreatPassword!;";
    var store = DocumentStore.For(options =>
    {
        options.Connection(connectionString);
        options.AutoCreateSchemaObjects = AutoCreate.All;
    });

    return store;
});
builder.Services.AddScoped<IDocumentSession>(_ => _.GetRequiredService<IDocumentStore>().LightweightSession());


builder.Services.AddScoped<IEntityEventsPublisher, EntityEventsPublisher>();

builder.Services.AddScoped<ISomethingRepository, SomethingRepository>();
builder.Services.AddScoped<IUnitOfWork, MartenUnitOfWork>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
